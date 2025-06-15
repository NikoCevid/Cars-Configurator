using Cars.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dao.Models;


namespace Cars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogService _logService;

        public LogsController(LogService logService)
        {
            _logService = logService;
        }

        // GET: api/logs/get/10
        [HttpGet("get/{n}")]
        [Authorize] 
        public ActionResult<IEnumerable<LogEntry>> GetLastN(int n)
        {
            if (n <= 0) return BadRequest("Broj zapisa mora biti veći od 0.");

            var logs = _logService.GetLast(n);
            return Ok(logs);
        }

        // GET: api/logs/count
        [HttpGet("count")]
        [Authorize] 
        public ActionResult<int> GetCount()
        {
            return Ok(_logService.Count());
        }
    }
}
