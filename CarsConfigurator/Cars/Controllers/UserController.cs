using AutoMapper;
using Cars.DTO;
using Cars.Services.Interfaces;
using Dao.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<UserDTO>>(data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserDTO dto)
        {
            var user = _mapper.Map<User>(dto);
            await _service.AddAsync(user);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserLoginDTO dto)
        {
            var user = await _service.GetByUsernameAsync(dto.Username);
            if (user == null || user.PwdHash != dto.Password)
                return Unauthorized("Invalid credentials");

            return Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
