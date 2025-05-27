using Cars.DTO;
using Dao.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentTypesController : ControllerBase
    {
        private readonly CarsContext _context;

        public ComponentTypesController(CarsContext context)
        {
            _context = context;
        }

        // GET: api/componenttypes
        [HttpGet]
        public ActionResult<IEnumerable<ComponentTypeDTO>> GetAll()
        {
            try
            {
                var types = _context.ComponentTypes
                    .Select(x => new ComponentTypeDTO
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .ToList();

                return Ok(types);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // GET: api/componenttypes/5
        [HttpGet("{id}")]
        public ActionResult<ComponentTypeDTO> GetById(int id)
        {
            try
            {
                var type = _context.ComponentTypes
                    .Where(x => x.Id == id)
                    .Select(x => new ComponentTypeDTO
                    {
                        Id = x.Id,
                        Name = x.Name
                    })
                    .FirstOrDefault();

                if (type == null)
                    return NotFound();

                return Ok(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // POST: api/componenttypes
        [HttpPost]
        public IActionResult Post(CreateComponentTypeDTO model)
        {
            try
            {
                var type = new ComponentType
                {
                    Name = model.Name
                };

                _context.ComponentTypes.Add(type);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = type.Id }, new ComponentTypeDTO
                {
                    Id = type.Id,
                    Name = type.Name
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // PUT: api/componenttypes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, CreateComponentTypeDTO model)
        {
            try
            {
                var existing = _context.ComponentTypes.Find(id);
                if (existing == null)
                    return NotFound();

                existing.Name = model.Name;
                _context.SaveChanges();

                return Ok(new ComponentTypeDTO
                {
                    Id = existing.Id,
                    Name = existing.Name
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // DELETE: api/componenttypes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var existing = _context.ComponentTypes.Find(id);
                if (existing == null)
                    return NotFound();

                _context.ComponentTypes.Remove(existing);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
