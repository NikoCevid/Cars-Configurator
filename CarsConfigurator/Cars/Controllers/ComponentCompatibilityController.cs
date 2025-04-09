using Cars.DTO;
using Cars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentCompatibilityController : ControllerBase
    {
        private readonly CarsContext _context;

        public ComponentCompatibilityController(CarsContext context)
        {
            _context = context;
        }

        [HttpGet("{componentId}")]
        public ActionResult<ComponentCompatibilityDTO> GetCompatibleComponents(int componentId)
        {
            var component = _context.CarComponents
                .Include(c => c.CarComponentCompatibilityCarComponentId1Navigations)
                    .ThenInclude(cc => cc.CarComponentId2Navigation)
                .FirstOrDefault(c => c.Id == componentId);

            if (component == null)
                return NotFound();

            var dto = new ComponentCompatibilityDTO
            {
                ComponentId = component.Id,
                ComponentName = component.Name,
                CompatibleWith = component.CarComponentCompatibilityCarComponentId1Navigations
                    .Select(cc => new CompatibleComponentDTO
                    {
                        Id = cc.CarComponentId2,
                        Name = cc.CarComponentId2Navigation.Name
                    }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        public IActionResult CreateCompatibility(CreateCompabilityDTO dto)
        {
            if (dto.ComponentId1 == dto.ComponentId2)
                return BadRequest("Komponenta ne može biti kompatibilna sama sa sobom.");

            var exists = _context.CarComponentCompatibilities.Any(c =>
                (c.CarComponentId1 == dto.ComponentId1 && c.CarComponentId2 == dto.ComponentId2) ||
                (c.CarComponentId1 == dto.ComponentId2 && c.CarComponentId2 == dto.ComponentId1));

            if (exists)
                return Conflict("Kompatibilnost već postoji.");

            var compatibility = new CarComponentCompatibility
            {
                CarComponentId1 = dto.ComponentId1,
                CarComponentId2 = dto.ComponentId2
            };

            _context.CarComponentCompatibilities.Add(compatibility);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteCompatibility([FromBody] CreateCompabilityDTO dto)
        {
            var item = _context.CarComponentCompatibilities.FirstOrDefault(c =>
                (c.CarComponentId1 == dto.ComponentId1 && c.CarComponentId2 == dto.ComponentId2) ||
                (c.CarComponentId1 == dto.ComponentId2 && c.CarComponentId2 == dto.ComponentId1));

            if (item == null)
                return NotFound("Kompatibilnost nije pronađena.");

            _context.CarComponentCompatibilities.Remove(item);
            _context.SaveChanges();

            return Ok();
        }
    }
}
