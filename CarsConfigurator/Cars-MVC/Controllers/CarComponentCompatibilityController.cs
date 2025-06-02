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

        // GET: CarComponentCompatibility
        public async Task<IActionResult> Index()
        {
            var list = await _context.CarComponentCompatibilities
              .Include(c => c.CarComponentId1Navigation)
.   Include(c => c.CarComponentId2Navigation)
                .ToListAsync();

            return View(list);
        }

        // GET: CarComponentCompatibility/Create
        public IActionResult Create()
        {
            ViewBag.Components = new SelectList(_context.CarComponents, "Id", "Name");
            return View();
        }

        // POST: CarComponentCompatibility/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarComponentCompatibility model)
        {
            if (ModelState.IsValid && model.CarComponentId1Navigation != model.CarComponentId2Navigation)
            {
                _context.CarComponentCompatibilities.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Components = new SelectList(_context.CarComponents, "Id", "Name");
            return View(model);
        }

        // GET: CarComponentCompatibility/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var compatibility = await _context.CarComponentCompatibilities
                .Include(c => c.CarComponentId1Navigation)
                .Include(c => c.CarComponentId2Navigation)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compatibility == null) return NotFound();

            return View(compatibility);
        }

        // POST: CarComponentCompatibility/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compatibility = await _context.CarComponentCompatibilities.FindAsync(id);
            if (compatibility != null)
            {
                _context.CarComponentCompatibilities.Remove(compatibility);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
