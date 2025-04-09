using Cars.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cars.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Cars.Security;

namespace Cars.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CarsContext _context;

        public UserController(IConfiguration configuration, CarsContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        // GET: api/User
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

        // GET: api/User/{id}
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

        // POST: api/User
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

        // PUT: api/User/{id}
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

        // DELETE: api/User/{id}
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

        // GET: api/User/GetToken
        [HttpGet("[action]")]
        public ActionResult GetToken()
        {
            try
            {
                var secureKey = _configuration["Jwt:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);
                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/User/Register
        [HttpPost("Register")]
        public ActionResult<CreateUserDTO> Register([FromBody] CreateUserDTO CreateUserDTO)
        {

            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState); 
            }


            try
            {
                var username = CreateUserDTO.Username.Trim();

                if (_context.Users.Any(x => x.Username == username) == true)
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

                // Update DTO Id to return it to the client
                CreateUserDTO.Id = user.Id;

                return Ok(CreateUserDTO);
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpPost("login")]

        public ActionResult Login([FromBody] UserLoginDTO UserLoginDTO)
        {
            try
            {
                var genericErrorMessage = "Incorrect username or password";

                if (ModelState.IsValid == false) return BadRequest(ModelState);

                var username = UserLoginDTO.Username.Trim();

                var user = _context.Users.FirstOrDefault(u => u.Username == username);

                if (user is null)
                {
                    return BadRequest(genericErrorMessage);
                }

                var targetHash = PasswordHashProvider.GetHash(UserLoginDTO.Password, user.PwdSalt);
                var sourceHash = user.PwdHash;

                if (targetHash != sourceHash)
                {
                    return BadRequest(genericErrorMessage);
                }

                var SecureKey = _configuration["Jwt:SecureKey"];
                var token = JwtTokenProvider.CreateToken(SecureKey, 10);
                return Ok(token);
            }
            catch (Exception)
            {

                return StatusCode(500); 
            }
        }

    }
}
