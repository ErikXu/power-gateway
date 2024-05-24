using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Mongo.Entities;
using WebApi.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;

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
        public async Task<IActionResult> CreateConfig([FromBody] AlarmConfigForm form)
        {
            var config = new AlarmConfig
            {
                Name = form.Name,
                Type = form.Type,
                BotUrl = form.BotUrl,
                CreateAt = DateTime.UtcNow
            };

            await _mongoDbContext.Collection<AlarmConfig>().InsertOneAsync(config);

            return Ok();
        }

        [HttpGet("configs")]
        public IActionResult GetConfigs()
        {
            var configs = _mongoDbContext.Collection<AlarmConfig>().AsQueryable().OrderByDescending(n => n.CreateAt).ToList();
            return Ok(configs);
        }

        [HttpPost("rules")]
        public async Task<IActionResult> CreateRule([FromBody] AlarmRuleForm form)
        {
            var rule = new AlarmRule
            {
                Title = form.Title,
                Field = form.Field,
                Operator = form.Operator,
                Value = form.Value,
                //AlarmConfigId = new ObjectId(form.AlarmConfigId)
            };

            await _mongoDbContext.Collection<AlarmRule>().InsertOneAsync(rule);

            return Ok();
        }
    }
}

