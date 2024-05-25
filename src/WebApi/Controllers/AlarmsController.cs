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
                BotUrl = form.BotUrl
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
                AlarmConfigId = new ObjectId(form.AlarmConfigId)
            };

            await _mongoDbContext.Collection<AlarmRule>().InsertOneAsync(rule);

            return Ok();
        }

        [HttpGet("rules")]
        public IActionResult GetRules()
        {
            var configDic = _mongoDbContext.Collection<AlarmConfig>()
                                           .AsQueryable()
                                           .ToList()
                                           .ToDictionary(key => key.Id, val => val.Name);

            var rules = _mongoDbContext.Collection<AlarmRule>()
                                       .AsQueryable()
                                       .OrderByDescending(n => n.CreateAt)
                                       .ToList()
                                       .Select(n => new AlarmRuleItem
                                       {
                                           Id = n.Id.ToString(),
                                           Title = n.Title,
                                           Field = n.Field,
                                           Operator = n.Operator,
                                           Value = n.Value,
                                           AlarmConfigId = n.AlarmConfigId.ToString(),
                                           AlarmConfigText = configDic[n.AlarmConfigId],
                                           CreateAt = n.CreateAt
                                       }).ToList();
            return Ok(rules);
        }

        [HttpGet("option/fields")]
        public IActionResult GetFieldOptions()
        {
            var options = new List<KeyValueItem<string, string>>()
            {
                new KeyValueItem<string, string>{Key = "status", Value = "Http Status"}
            };

            return Ok(options);
        }

        [HttpGet("option/operators")]
        public IActionResult GetOperatorOptions()
        {
            var options = new List<KeyValueItem<string, string>>()
            {
                new KeyValueItem<string, string>{Key = "==", Value = "Equals"}
            };

            return Ok(options);
        }

        [HttpGet("option/configs")]
        public IActionResult GetConfigOptions()
        {
            var options = _mongoDbContext.Collection<AlarmConfig>()
                                         .AsQueryable()
                                         .Select(n => new KeyValueItem<string, string> { Key = n.Id.ToString(), Value = n.Name })
                                         .ToList();

            return Ok(options);
        }
    }
}

