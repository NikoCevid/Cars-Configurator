using Cars_MVC.Models;
using Dao.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Cars_MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly CarsContext _context;

        public AdminController(CarsContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index(string search, string roleFilter, int page = 1)
        {
            int pageSize = 10;
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(u => u.Username.Contains(search));

            if (!string.IsNullOrWhiteSpace(roleFilter))
                query = query.Where(u => u.Role == roleFilter);

            ViewBag.Roles = new SelectList(await _context.Users.Select(u => u.Role).Distinct().ToListAsync());
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentRole = roleFilter;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)await query.CountAsync() / pageSize);

            var users = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new AdminUserListViewModel
            {
                Users = users
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        public async Task<IActionResult> UserConfigurations()
        {
            var data = await _context.Users
                .Include(u => u.Configurations)
                    .ThenInclude(c => c.ConfigurationCarComponents)
                    .ThenInclude(cc => cc.CarComponent)
                .ToListAsync();

            var result = data.Select(u => new UserConfigurationDTO
            {
                Username = u.Username,
                ComponentNames = u.Configurations
                    .SelectMany(conf => conf.ConfigurationCarComponents)
                    .Select(cc => cc.CarComponent.Name)
                    .Distinct()
                    .ToList()
            }).ToList();

            return View(result);
        }
    }
}
