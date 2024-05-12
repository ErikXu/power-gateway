using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("apisix")]
        public IActionResult SaveApisixLog([FromBody] ApisixLogRequest request)
        {
            _logger.LogInformation(request.ToString());
            return Ok();
        }
    }
}
