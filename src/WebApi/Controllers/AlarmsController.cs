using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Mongo.Entities;
using WebApi.Mongo;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;

        public AlarmsController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpPost("configs")]
        public async Task<IActionResult> Create([FromBody] AlarmConfigForm form)
        {
            var config = new AlarmConfig
            {
                Name = form.Name,
                Type = form.Type,
                BotUrl = form.BotUrl
            };

            await _mongoDbContext.Collection<AlarmConfig>().InsertOneAsync(config);

            return Ok();
        }
    }
}

