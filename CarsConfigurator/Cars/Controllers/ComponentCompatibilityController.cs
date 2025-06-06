﻿using AutoMapper;
using Cars.DTO;
using Cars.Services.Interfaces;
using Dao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentCompatibilityController : ControllerBase
    {
        private readonly ICarComponentCompatibilityService _service;
        private readonly IMapper _mapper;

        public ComponentCompatibilityController(ICarComponentCompatibilityService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentCompatibilityDTO>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<ComponentCompatibilityDTO>>(data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComponentCompatibilityDTO>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(_mapper.Map<ComponentCompatibilityDTO>(item));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCompabilityDTO dto)
        {
            var entity = _mapper.Map<CarComponentCompatibility>(dto);
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
