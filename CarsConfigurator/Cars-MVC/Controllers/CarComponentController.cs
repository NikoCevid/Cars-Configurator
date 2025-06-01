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

        // GET: CarComponent

public async Task<IActionResult> Index(CarComponentFilterViewModel filter)
    {
        int pageSize = 10;

        var query = _context.CarComponents
            .Include(c => c.ComponentType)
            .AsQueryable();

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
         .Select(c => new SelectListItem
         {
             Value = c.Id.ToString(),
             Text = c.Name
         })
         .ToList();
            ViewBag.Filter = filter;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        return View(components);
    }


    // GET: CarComponent/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var component = await _context.CarComponents
                .Include(c => c.ComponentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (component == null) return NotFound();

            return View(component);
        }

        // GET: CarComponent/Create
        public IActionResult Create()
        {
            ViewData["ComponentTypeId"] = new SelectList(_context.ComponentTypes, "Id", "Name");
            return View();
        }

        // POST: CarComponent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarComponent component, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    var savePath = Path.Combine("wwwroot/uploads", fileName);
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    component.ImagePath = "/uploads/" + fileName;
                }

                _context.Add(component);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ComponentTypeId"] = new SelectList(_context.ComponentTypes, "Id", "Name", component.ComponentTypeId);
            return View(component);
        }

        // GET: CarComponent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var component = await _context.CarComponents.FindAsync(id);
            if (component == null) return NotFound();

            ViewData["ComponentTypeId"] = new SelectList(_context.ComponentTypes, "Id", "Name", component.ComponentTypeId);
            return View(component);
        }

        //  POST: CarComponent/Edit/5 — s podrškom za upload nove slike
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarComponent component, IFormFile? file)
        {
            if (id != component.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var path = Path.Combine("wwwroot/uploads", fileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                            await file.CopyToAsync(stream);

                        component.ImagePath = "/uploads/" + fileName;
                    }

                    _context.Update(component);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CarComponents.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ComponentTypeId"] = new SelectList(_context.ComponentTypes, "Id", "Name", component.ComponentTypeId);
            return View(component);
        }

        // GET: CarComponent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var component = await _context.CarComponents
                .Include(c => c.ComponentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (component == null) return NotFound();

            return View(component);
        }

        // POST: CarComponent/Delete/5
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
