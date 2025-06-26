using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dao.Models;
using Cars_MVC.Models;

namespace Cars_MVC.Controllers
{
    public class CarComponentController : Controller
    {
        private readonly CarsContext _context;

        public CarComponentController(CarsContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(CarComponentFilterViewModel filter)
        {
            int pageSize = 6;
            var query = _context.CarComponents.Include(c => c.ComponentType).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                query = query.Where(c => c.Name.Contains(filter.SearchTerm));

            if (filter.ComponentTypeId.HasValue)
                query = query.Where(c => c.ComponentTypeId == filter.ComponentTypeId);

            int totalItems = await query.CountAsync();
            var components = await query
                .Skip((filter.Page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.ComponentTypes = _context.ComponentTypes
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();

            ViewBag.Filter = filter;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(components);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var component = await _context.CarComponents
                .Include(c => c.ComponentType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (component == null) return NotFound();

            return View(component);
        }

    
        public IActionResult Create()
        {
            ViewBag.ComponentTypes = new SelectList(_context.ComponentTypes.ToList(), "Id", "Name");
            return View(new CarComponentUploadViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarComponentUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ComponentTypes = new SelectList(_context.ComponentTypes.ToList(), "Id", "Name", model.ComponentTypeId);
                return View(model);
            }

            var component = new CarComponent
            {
                Name = model.Name,
                Description = model.Description,
                ComponentTypeId = model.ComponentTypeId
            };

            if (model.Image != null && model.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.Image.CopyToAsync(ms);
                    byte[] imageBytes = ms.ToArray();
                    component.ImageBase64 = Convert.ToBase64String(imageBytes);
                }
            }

            _context.CarComponents.Add(component);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var component = await _context.CarComponents.FindAsync(id);
            if (component == null) return NotFound();

            var model = new CarComponentUploadViewModel
            {
                Name = component.Name,
                Description = component.Description,
                ComponentTypeId = component.ComponentTypeId
            };

            ViewBag.ComponentTypes = new SelectList(_context.ComponentTypes.ToList(), "Id", "Name", component.ComponentTypeId);
            ViewBag.ComponentId = component.Id;
            ViewBag.ExistingImage = component.ImageBase64;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarComponentUploadViewModel model)
        {
            var component = await _context.CarComponents.FindAsync(id);
            if (component == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.ComponentTypes = new SelectList(_context.ComponentTypes.ToList(), "Id", "Name", model.ComponentTypeId);
                ViewBag.ComponentId = id;
                ViewBag.ExistingImage = component.ImageBase64;
                return View(model);
            }

            component.Name = model.Name;
            component.Description = model.Description;
            component.ComponentTypeId = model.ComponentTypeId;

            if (model.Image != null && model.Image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.Image.CopyToAsync(ms);
                    byte[] imageBytes = ms.ToArray();
                    component.ImageBase64 = Convert.ToBase64String(imageBytes);
                }
            }

            _context.Update(component);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var component = await _context.CarComponents
                .Include(c => c.ComponentType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (component == null) return NotFound();

            return View(component);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var component = await _context.CarComponents.FindAsync(id);
            if (component != null)
            {
                _context.CarComponents.Remove(component);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
