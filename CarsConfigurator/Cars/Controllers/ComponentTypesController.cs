using AutoMapper;
using Cars.DTO;
using Cars.Services.Interfaces;
using Dao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentTypesController : ControllerBase
    {
        private readonly IComponentTypeService _service;
        private readonly IMapper _mapper;

        public ComponentTypesController(IComponentTypeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentTypeDTO>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<ComponentTypeDTO>>(data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComponentTypeDTO>> GetById(int id)
        {
            var type = await _service.GetByIdAsync(id);
            if (type == null) return NotFound();
            return Ok(_mapper.Map<ComponentTypeDTO>(type));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateComponentTypeDTO dto)
        {
            var entity = _mapper.Map<ComponentType>(dto);
            await _service.AddAsync(entity);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ComponentTypeDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = _mapper.Map<ComponentType>(dto);
            await _service.UpdateAsync(entity);
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
