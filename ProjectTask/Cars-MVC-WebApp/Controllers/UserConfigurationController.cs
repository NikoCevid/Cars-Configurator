using Cars_MVC.Models;
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

 
        public async Task<IActionResult> Choose()
        {
            var components = await _context.CarComponents.Include(c => c.ComponentType).ToListAsync();
            ViewBag.ComponentTypes = await _context.ComponentTypes.ToListAsync();
            return View(components);
        }

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
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null) return Unauthorized();

            var model = user.Configurations
                .OrderByDescending(c => c.CreationDate)
                .Select(config => new UserConfigurationGroupedViewModel
                {
                    UserName = user.Username,
                    CreatedAt = config.CreationDate,
                    ComponentNames = string.Join(", ", config.ConfigurationCarComponents
                        .Select(cc => cc.CarComponent.Name))
                })
                .ToList();

            return View(model);
        }



    }
}
