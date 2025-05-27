using AutoMapper;
using Cars.DTO;
using Cars.Services.Interfaces;
using Dao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationsController : ControllerBase
    {
        private readonly IConfigurationService _service;
        private readonly IMapper _mapper;

        public ConfigurationsController(IConfigurationService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConfigurationDTO>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<ConfigurationDTO>>(data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConfigurationDTO>> GetById(int id)
        {
            var config = await _service.GetByIdAsync(id);
            if (config == null) return NotFound();
            return Ok(_mapper.Map<ConfigurationDTO>(config));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ConfigurationDTO dto)
        {
            var entity = _mapper.Map<Configuration>(dto);
            await _service.AddAsync(entity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
