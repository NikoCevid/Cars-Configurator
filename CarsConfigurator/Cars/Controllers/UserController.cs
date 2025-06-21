using AutoMapper;
using Cars.DTO;
using Cars.Security;
using Cars.Services;
using Cars.Services.Interfaces;
using Dao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly LogService _logService;
        private readonly string _jwtKey;

        public UserController(IUserService service,
                              IMapper mapper,
                              IConfiguration config,
                              LogService logService)
        {
            _service = service;
            _mapper = mapper;
            _logService = logService;
            _jwtKey = config["Jwt:SecureKey"]!;
        }

        

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] CreateUserDTO dto)
        {
            if (await _service.GetByUsernameAsync(dto.Username) is not null)
                return BadRequest("Korisničko ime već postoji.");

            CreatePasswordHash(dto.Password, out string hash, out string salt);

            var user = _mapper.Map<User>(dto);
            user.PwdHash = hash;
            user.PwdSalt = salt;
            user.Role = "User";

            await _service.AddAsync(user);

            _logService.Log("INFO", $"Korisnik {dto.Username} je registriran.");
            return Ok("Registracija uspješna.");
        }

      

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] UserLoginDTO dto)
        {
            var user = await _service.GetByUsernameAsync(dto.Username);
            if (user is null || !VerifyPassword(dto.Password, user.PwdHash, user.PwdSalt))
            {
                _logService.Log("WARN", $"Neuspješan login za korisnika {dto.Username}.");
                return Unauthorized("Pogrešno korisničko ime ili lozinka.");
            }

            var token = JwtTokenProvider.CreateToken(_jwtKey, 120, user.Username);

            _logService.Log("INFO", $"Korisnik {user.Username} se uspješno prijavio.");

            return Ok(new
            {
                token,
                user = _mapper.Map<UserDTO>(user)
            });
        }

       

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(_mapper.Map<List<UserDTO>>(data));
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user is null) return NotFound();

            return Ok(_mapper.Map<UserDTO>(user));
        }



        private static void CreatePasswordHash(string password,
                                               out string passwordHash,
                                               out string passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private static bool VerifyPassword(string password,
                                           string storedHash,
                                           string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var expectedHash = Convert.FromBase64String(storedHash);

            using var hmac = new HMACSHA512(saltBytes);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(expectedHash);
        }
    }
}
