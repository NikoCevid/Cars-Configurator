using Cars.DTO;
using Dao.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private readonly CarsContext _context;

        public ConfigurationsController(CarsContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ConfigurationDTO>> GetAll()
        {
            try
            {
                var configurations = _context.Configurations
                    .Include(c => c.ConfigurationCarComponents)
                        .ThenInclude(cc => cc.CarComponent)
                    .Select(c => new ConfigurationDTO
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        CreationDate = c.CreationDate,
                        ConfigurationCarComponents = c.ConfigurationCarComponents.Select(cc => new ConfigurationCarComponentDTO
                        {
                            Id = cc.Id,
                            CarComponentId = cc.CarComponentId,
                            CarComponentName = cc.CarComponent.Name
                        }).ToList()
                    })
                    .ToList();

                return Ok(configurations);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post(ConfigurationDTO model)
        {
            try
            {
                var configuration = new Configuration
                {
                    UserId = model.UserId,
                    CreationDate = DateTime.Now
                };

                _context.Configurations.Add(configuration);
                _context.SaveChanges();

                foreach (var item in model.ConfigurationCarComponents)
                {
                    var configCarComponent = new ConfigurationCarComponent
                    {
                        ConfigurationId = configuration.Id,
                        CarComponentId = item.CarComponentId
                    };

                    _context.ConfigurationCarComponents.Add(configCarComponent);
                }

                _context.SaveChanges();

                return CreatedAtAction(nameof(GetBy), new { id = configuration.Id }, configuration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, ConfigurationDTO model)
        {
            try
            {
                var existing = _context.Configurations
                    .Include(c => c.ConfigurationCarComponents)
                    .FirstOrDefault(c => c.Id == id);

                if (existing == null) return NotFound();

                existing.UserId = model.UserId;
                existing.CreationDate = model.CreationDate;

                _context.ConfigurationCarComponents.RemoveRange(existing.ConfigurationCarComponents);

                foreach (var item in model.ConfigurationCarComponents)
                {
                    var configCarComponent = new ConfigurationCarComponent
                    {
                        ConfigurationId = existing.Id,
                        CarComponentId = item.CarComponentId
                    };

                    _context.ConfigurationCarComponents.Add(configCarComponent);
                }

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
                var item = _context.Configurations.Find(id);
                if (item == null) return NotFound();

                _context.Configurations.Remove(item);
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
        public ActionResult<ConfigurationDTO> GetBy(int id)
        {
            try
            {
                var configuration = _context.Configurations
                    .Include(c => c.ConfigurationCarComponents)
                        .ThenInclude(cc => cc.CarComponent)
                    .Where(c => c.Id == id)
                    .Select(c => new ConfigurationDTO
                    {
                        Id = c.Id,
                        UserId = c.UserId,
                        CreationDate = c.CreationDate,
                        ConfigurationCarComponents = c.ConfigurationCarComponents.Select(cc => new ConfigurationCarComponentDTO
                        {
                            Id = cc.Id,
                            CarComponentId = cc.CarComponentId,
                            CarComponentName = cc.CarComponent.Name
                        }).ToList()
                    })
                    .FirstOrDefault();

                if (configuration == null)
                    return NotFound();

                return Ok(configuration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        // ✅ NOVA METODA: dodavanje komponente u konfiguraciju s provjerom kompatibilnosti
        [HttpPost("{configurationId}/add-component/{componentId}")]
        public async Task<IActionResult> AddCarComponentToConfiguration(int configurationId, int componentId)
        {
            var configuration = await _context.Configurations
                .Include(c => c.ConfigurationCarComponents)
                .FirstOrDefaultAsync(c => c.Id == configurationId);

            if (configuration == null)
            {
                return NotFound($"Configuration with ID {configurationId} not found.");
            }

            var newComponent = await _context.CarComponents.FindAsync(componentId);
            if (newComponent == null)
            {
                return NotFound($"Car component with ID {componentId} not found.");
            }

            var existingComponentIds = configuration.ConfigurationCarComponents
                .Select(ccc => ccc.CarComponentId)
                .ToList();  

               bool incompatible = await _context.CarComponentCompatibilities.AnyAsync(cc =>
    (cc.CarComponentId1 == componentId && existingComponentIds.Contains(cc.CarComponentId2)) ||
    (cc.CarComponentId2 == componentId && existingComponentIds.Contains(cc.CarComponentId1)));


            if (incompatible)
            {
                return BadRequest("Selected component is incompatible with one or more components in the configuration.");
            }

            var configComponent = new ConfigurationCarComponent
            {
                ConfigurationId = configurationId,
                CarComponentId = componentId
            };

            _context.ConfigurationCarComponents.Add(configComponent);
            await _context.SaveChangesAsync();

            return Ok("Component successfully added to configuration.");
        }
    }
}
