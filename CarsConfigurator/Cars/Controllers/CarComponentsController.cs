using Cars.DTO;
using Cars.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarComponentsController : ControllerBase
    {
        private readonly CarsContext _context;

        public CarComponentsController(CarsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CarComponentDTO>> GetAll()
        {
            try
            {
                var components = _context.CarComponents
                    .Select(x => new CarComponentDTO    
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ComponentTypeName = x.ComponentType.Name,
                        ImagePath = x.ImagePath
                    })
                    .ToList();

                return Ok(components);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post(CreateCarComponentDTO model)
        {
            try
            {
                var carComponent = new CarComponent
                {
                    Name = model.Name,
                    Description = model.Description,
                    ComponentTypeId = model.ComponentTypeId,
                    ImagePath = model.ImagePath
                };

                _context.CarComponents.Add(carComponent);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetBy), new { id = carComponent.Id }, carComponent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, CreateCarComponentDTO model)
        {
            try
            {
                var existing = _context.CarComponents.Find(id);
                if (existing == null) return NotFound();

                existing.Name = model.Name;
                existing.Description = model.Description;
                existing.ComponentTypeId = model.ComponentTypeId;
                existing.ImagePath = model.ImagePath;

                _context.SaveChanges();
                return Ok(existing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var item = _context.CarComponents.Find(id);
                if (item == null) return NotFound(BadRequest("Component type does not exist"));

                _context.CarComponents.Remove(item);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CarComponentDTO> GetBy(int id)
        {
            try
            {
                var component = _context.CarComponents
                    .Where(x => x.Id == id)
                    .Select(x => new CarComponentDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ComponentTypeName = x.ComponentType.Name,
                        ImagePath = x.ImagePath
                    })
                    .FirstOrDefault();

                if (component == null)
                    return NotFound(BadRequest("Component type does not exist"));

                return Ok(component);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
