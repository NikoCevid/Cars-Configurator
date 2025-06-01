using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dao.Models;

namespace Cars_MVC.Controllers
{
    public class CarComponentTypeController : Controller
    {
        private readonly CarsContext _context;

        public CarComponentTypeController(CarsContext context)
        {
            _context = context;
        }

        // GET: ComponentType
        public async Task<IActionResult> Index()
        {
            return View(await _context.ComponentTypes.ToListAsync());
        }

        // GET: ComponentType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var componentType = await _context.ComponentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componentType == null) return NotFound();

            return View(componentType);
        }

        // GET: ComponentType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ComponentType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComponentType componentType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(componentType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(componentType);
        }

        // GET: ComponentType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var componentType = await _context.ComponentTypes.FindAsync(id);
            if (componentType == null) return NotFound();

            return View(componentType);
        }

        // POST: ComponentType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ComponentType componentType)
        {
            if (id != componentType.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(componentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ComponentTypes.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(componentType);
        }

        // GET: ComponentType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var componentType = await _context.ComponentTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (componentType == null) return NotFound();

            return View(componentType);
        }

        // POST: ComponentType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var componentType = await _context.ComponentTypes.FindAsync(id);
            if (componentType != null)
            {
                _context.ComponentTypes.Remove(componentType);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
