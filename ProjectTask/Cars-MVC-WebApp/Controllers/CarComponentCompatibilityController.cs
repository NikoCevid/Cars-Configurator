using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dao.Models;

namespace Cars_MVC.Controllers
{
    public class CarComponentCompatibilityController : Controller
    {
        private readonly CarsContext _context;

        public CarComponentCompatibilityController(CarsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _context.CarComponentCompatibilities
                .Include(x => x.CarComponentId1Navigation)
                .Include(x => x.CarComponentId2Navigation)
                .ToListAsync();

            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Components = new SelectList(_context.CarComponents.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarComponentCompatibility model)
        {
            if (ModelState.IsValid && model.CarComponentId1 != model.CarComponentId2)
            {
                _context.CarComponentCompatibilities.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Ne možete odabrati istu komponentu za obje strane.");
            ViewBag.Components = new SelectList(_context.CarComponents.ToList(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Delete(int id1, int id2)
        {
            var item = await _context.CarComponentCompatibilities
                .Include(c => c.CarComponentId1Navigation)
                .Include(c => c.CarComponentId2Navigation)
                .FirstOrDefaultAsync(c => c.CarComponentId1 == id1 && c.CarComponentId2 == id2);

            if (item == null)
                return NotFound();

            _context.CarComponentCompatibilities.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Configure()
        {
            ViewBag.AllComponents = _context.CarComponents.ToList();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCompatibleComponents(int id)
        {
            var compatibleIds = await _context.CarComponentCompatibilities
                .Where(c => c.CarComponentId1 == id || c.CarComponentId2 == id)
                .Select(c => c.CarComponentId1 == id ? c.CarComponentId2 : c.CarComponentId1)
                .ToListAsync();

            var compatibleComponents = await _context.CarComponents
                .Where(c => compatibleIds.Contains(c.Id))
                .ToListAsync();

            return Json(compatibleComponents);
        }
    }
}
