using Dao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cars_MVC.Controllers
{
    public class UserConfigurationController : Controller
    {
        private readonly CarsContext _context;

        public UserConfigurationController(CarsContext context)
        {
            _context = context;
        }

        // GET: /UserConfiguration/Choose
        public async Task<IActionResult> Choose()
        {
            var components = await _context.CarComponents.Include(c => c.ComponentType).ToListAsync();
            ViewBag.ComponentTypes = await _context.ComponentTypes.ToListAsync();
            return View(components);
        }

        // POST: /UserConfiguration/Save
        [HttpPost]
        public async Task<IActionResult> Save(List<int> componentIds)
        {
            if (!componentIds.Any()) return RedirectToAction("Choose");

            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return Unauthorized();

            var config = new Configuration
            {
                UserId = user.Id,
                CreationDate = DateTime.Now
            };

            _context.Configurations.Add(config);
            await _context.SaveChangesAsync();

            foreach (var componentId in componentIds)
            {
                _context.ConfigurationCarComponents.Add(new ConfigurationCarComponent
                {
                    ConfigurationId = config.Id,
                    CarComponentId = componentId
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("MyConfiguration");
        }

        public async Task<IActionResult> MyConfiguration()
        {
            var username = User.Identity?.Name;

            var user = await _context.Users
                .Include(u => u.Configurations)
                    .ThenInclude(c => c.ConfigurationCarComponents)
                        .ThenInclude(cc => cc.CarComponent)
                            .ThenInclude(comp => comp.ComponentType)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return Unauthorized();

            var lastConfig = user.Configurations
                .OrderByDescending(c => c.Id)
                .FirstOrDefault();

            ViewBag.ConfigDate = lastConfig?.CreationDate;

            var components = lastConfig?
                .ConfigurationCarComponents
                .Select(ccc => ccc.CarComponent)
                .ToList() ?? new List<CarComponent>();

            return View(components); // Gleda Views/UserConfiguration/MyConfiguration.cshtml
        }

    }
}
