using Cars_MVC.Models;
using Dao.Models;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Index(string SearchUsername, string SearchRole, int Page = 1, int PageSize = 10)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchUsername))
                query = query.Where(u => u.Username.Contains(SearchUsername));

            if (!string.IsNullOrWhiteSpace(SearchRole))
                query = query.Where(u => u.Role == SearchRole);

            var roles = await _context.Users
                .Select(u => u.Role)
                .Distinct()
                .ToListAsync();

            var totalUsers = await query.CountAsync();
            var users = await query
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var viewModel = new AdminUserListViewModel
            {
                Users = users,
                AvailableRoles = roles,
                SearchUsername = SearchUsername,
                SearchRole = SearchRole,
                CurrentPage = Page,
                TotalPages = (int)Math.Ceiling((double)totalUsers / PageSize),
                PageSize = PageSize
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
            if (id != user.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
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

        public async Task<IActionResult> UserConfigurations(int Page = 1, int PageSize = 10)
        {
            var users = await _context.Users
                .Include(u => u.Configurations)
                    .ThenInclude(c => c.ConfigurationCarComponents)
                        .ThenInclude(cc => cc.CarComponent)
                .ToListAsync();

            var allConfigurations = users
                .SelectMany(u => u.Configurations.Select(config => new UserConfigurationGroupedViewModel
                {
                    UserName = u.Username,
                    CreatedAt = config.CreationDate,
                    ComponentNames = string.Join(", ", config.ConfigurationCarComponents
                        .Select(cc => cc.CarComponent.Name))
                }))
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            ViewBag.CurrentPage = Page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)allConfigurations.Count / PageSize);
            ViewBag.PageSize = PageSize;

            var pagedConfigurations = allConfigurations
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return View(pagedConfigurations);
        }



        // GET: Admin/Profile
        public async Task<IActionResult> Profile()
        {
            var username = User.Identity?.Name;
            if (username == null) return RedirectToAction("Login", "Auth");

            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Role == "Admin");
            if (admin == null) return NotFound();

            return View(admin);
        }

        // POST: Admin/UpdateProfile
        [HttpPost]
        public async Task<IActionResult> UpdateProfile([FromBody] User updatedUser)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Role == "Admin");
            if (user == null)
                return Json(new { success = false, message = "Admin nije pronađen." });

            // Validacija na serveru (dodatno)
            if (string.IsNullOrWhiteSpace(updatedUser.FirstName) ||
                string.IsNullOrWhiteSpace(updatedUser.LastName) ||
                string.IsNullOrWhiteSpace(updatedUser.Email) ||
                string.IsNullOrWhiteSpace(updatedUser.Phone))
            {
                return Json(new { success = false, message = "Sva polja moraju biti ispunjena." });
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Profil uspješno ažuriran." });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser)
        {
            var user = await _context.Users.FindAsync(updatedUser.Id);
            if (user == null)
                return Json(new { success = false, message = "Korisnik nije pronađen." });

            // Ažuriraj podatke
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Phone = updatedUser.Phone;
            user.Role = updatedUser.Role;

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Korisnik uspješno ažuriran." });
        }


    }
}
