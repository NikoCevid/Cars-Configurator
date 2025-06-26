using AutoMapper;
using Cars.DTO;
using Cars.Services.Interfaces;
using Dao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarComponentController : ControllerBase
    {
        private readonly ICarComponentService _service;
        private readonly IMapper _mapper;

        public CarComponentController(ICarComponentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarComponentDTO>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<CarComponentDTO>>(data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarComponentDTO>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(_mapper.Map<CarComponentDTO>(item));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCarComponentDTO dto)
        {
            var existing = await _service.SearchAsync(dto.Name, 1, 1);
            if (existing.Any(c => c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { message = "Komponenta s ovim nazivom već postoji." });
            }

            var entity = _mapper.Map<CarComponent>(dto);
            await _service.AddAsync(entity);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CarComponentDTO dto)
        {
            if (id != dto.Id) return BadRequest();

           
            var existing = await _service.SearchAsync(dto.Name, 1, 10);
            if (existing.Any(c => c.Id != dto.Id && c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest(new { message = "Druga komponenta već koristi ovaj naziv." });
            }

            var entity = _mapper.Map<CarComponent>(dto);
            await _service.UpdateAsync(entity);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CarComponentDTO>>> Search(
    string? name = null,
    int page = 1,
    int pageSize = 10)
        {
            var result = await _service.SearchAsync(name, page, pageSize);
            return Ok(_mapper.Map<List<CarComponentDTO>>(result));
        }



    }
}
