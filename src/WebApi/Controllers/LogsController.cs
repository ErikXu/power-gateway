using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApi.Mongo;
using WebApi.Mongo.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly ILogger<LogsController> _logger;

        public LogsController(MongoDbContext mongoDbContext, ILogger<LogsController> logger)
        {
            _mongoDbContext = mongoDbContext;
            _logger = logger;
        }

        [HttpPost("apisix")]
        public async Task<IActionResult> SaveApisixLog()
        {
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var postData = await reader.ReadToEndAsync();
                var requests = JsonConvert.DeserializeObject<List<ApisixLogRequest>>(postData);
                await _mongoDbContext.Collection<ApisixLogRequest>().InsertManyAsync(requests);
            }
            return Ok();
        }

        [HttpPost("apisix/raw")]
        public async Task<IActionResult> SaveRawApisixLog()
        {
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var postData = await reader.ReadToEndAsync();
                _logger.LogInformation(postData);
            }
            return Ok();
        }
    }
}
