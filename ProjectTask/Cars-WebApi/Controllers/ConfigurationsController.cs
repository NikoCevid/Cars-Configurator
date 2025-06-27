using AutoMapper;
using Cars.DTO;
using Cars.Services.Interfaces;
using Dao.Models;
using Microsoft.AspNetCore.Authorization;
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

        // POST: api/Configurations
        [HttpPost]
        public async Task<ActionResult<ConfigurationDTO>> Post(ConfigurationDTO dto)
        {
            var config = _mapper.Map<Configuration>(dto);

            config.Id = 0; // ensure EF Core auto-generates it
            foreach (var comp in config.ConfigurationCarComponents)
            {
                comp.Id = 0;
                comp.ConfigurationId = 0; // EF will set it on save
            }

            await _service.AddAsync(config);

            var result = _mapper.Map<ConfigurationDTO>(config);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        //[Authorize]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ConfigurationDTO>>> Search(string? query = "", int page = 1, int pageSize = 10)
        {
            var configs = await _service.SearchAsync(query, page, pageSize);
            return Ok(_mapper.Map<List<ConfigurationDTO>>(configs));
        }


    }
}
