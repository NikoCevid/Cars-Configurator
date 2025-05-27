using Dao.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cars.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Cars.Security;
using Cars.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Cars.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CarsContext _context;
        private readonly LogService _logService;

        public UserController(IConfiguration configuration, CarsContext context, LogService logService)
        {
            _configuration = configuration;
            _context = context;
            _logService = logService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAll()
        {
            try
            {
                var users = _context.Users
                    .Select(x => new UserDTO
                    {
                        Id = x.Id,
                        FullName = x.FirstName + " " + x.LastName,
                        Email = x.Email
                    })
                    .ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetBy(int id)
        {
            try
            {
                var user = _context.Users
                    .Where(x => x.Id == id)
                    .Select(x => new UserDTO
                    {
                        Id = x.Id,
                        FullName = x.FirstName + " " + x.LastName,
                        Email = x.Email
                    })
                    .FirstOrDefault();

                if (user is null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateUserDTO model)
        {
            try
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok(new { user.Id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CreateUserDTO model)
        {
            try
            {
                var existing = _context.Users.Find(id);
                if (existing == null) return NotFound();

                existing.FirstName = model.FirstName;
                existing.LastName = model.LastName;
                existing.Email = model.Email;
                existing.Phone = model.Phone;

                _context.SaveChanges();
                return Ok();
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
                var item = _context.Users.Find(id);
                if (item == null) return NotFound();

                _context.Users.Remove(item);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetToken()
        {
            try
            {
                var secureKey = _configuration["Jwt:SecureKey"];

                if (string.IsNullOrWhiteSpace(secureKey))
                    return BadRequest("JWT ključ nije definiran."); 

                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);
                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("Register")]
        public ActionResult<CreateUserDTO> Register([FromBody] CreateUserDTO CreateUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var username = CreateUserDTO.Username.Trim();

                if (_context.Users.Any(x => x.Username == username))
                    return BadRequest("Username already taken.");

                var salt = PasswordHashProvider.GetSalt();
                var passwordHash = PasswordHashProvider.GetHash(CreateUserDTO.Password, salt);

                var user = new User
                {
                    Id = CreateUserDTO.Id,
                    Username = CreateUserDTO.Username,
                    PwdHash = passwordHash,
                    PwdSalt = salt,
                    FirstName = CreateUserDTO.FirstName,
                    LastName = CreateUserDTO.LastName,
                    Email = CreateUserDTO.Email,
                    Phone = CreateUserDTO.Phone,
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                CreateUserDTO.Id = user.Id;
                return Ok(CreateUserDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginDTO UserLoginDTO)
        {
            try
            {
                var genericErrorMessage = "Incorrect username or password";

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var username = UserLoginDTO.Username.Trim();
                var user = _context.Users.FirstOrDefault(u => u.Username == username);

                if (user is null)
                    return BadRequest(genericErrorMessage);

                var targetHash = PasswordHashProvider.GetHash(UserLoginDTO.Password, user.PwdSalt);
                var sourceHash = user.PwdHash;

                if (targetHash != sourceHash)
                    return BadRequest(genericErrorMessage);

                var SecureKey = _configuration["Jwt:SecureKey"];
                var token = JwtTokenProvider.CreateToken(SecureKey, 10);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO model)
        {
            try
            {
                var username = User.FindFirstValue(ClaimTypes.Name);
                if (username == null)
                    return Unauthorized();

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                    return NotFound("Korisnik nije pronađen.");

                var isValid = PasswordHashProvider.VerifyPassword(model.CurrentPassword, user.PwdHash, user.PwdSalt);
                if (!isValid)
                {
                    _logService.Log("WARN", $"Pogrešan pokušaj promjene lozinke za korisnika '{username}'.");
                    return BadRequest("Trenutna lozinka nije ispravna.");
                }

                var (newHash, newSalt) = PasswordHashProvider.HashPassword(model.NewPassword);
                user.PwdHash = newHash;
                user.PwdSalt = newSalt;

                await _context.SaveChangesAsync();

                _logService.Log("INFO", $"Korisnik '{username}' je uspješno promijenio lozinku.");
                return Ok("Lozinka je uspješno promijenjena.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Došlo je do greške na serveru.");
            }
        }
    }
}
