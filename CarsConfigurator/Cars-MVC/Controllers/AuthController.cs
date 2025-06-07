using Cars_MVC.Models;
using Dao.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cars_MVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly CarsContext _context;

        public AuthController(CarsContext context)
        {
            _context = context;
        }

        // GET: Auth/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Korisničko ime je zauzeto.");
                return View(model);
            }

            //Generira se nasumični salt
            var salt = Guid.NewGuid().ToString(); // Generira random salt

            //Kreira se hash kombinacijom lozinke + salt, koristi se Base64 encoding
            var hash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.Password + salt));

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Phone = model.Phone,
                PwdSalt = salt,
                PwdHash = hash,
                Role = "user" // korisnik po defaultu nije admin
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction(nameof(Login));
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka.");
                return View(model);
            }

            var hash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.Password + user.PwdSalt));
            if (hash != user.PwdHash)
            {
                ModelState.AddModelError("", "Pogrešno korisničko ime ili lozinka.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(identity));

            return RedirectToAction("Index", "Home");
        }

        // POST: Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction(nameof(Login));
        }
    }
}
