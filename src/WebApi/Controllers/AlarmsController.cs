using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Mongo.Entities;
using WebApi.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlarmsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly ILarkService _larkService;

        public AlarmsController(MongoDbContext mongoDbContext, ILarkService larkService)
        {
            _mongoDbContext = mongoDbContext;
            _larkService = larkService;
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
            var configs = _mongoDbContext.Collection<AlarmConfig>()
                                         .AsQueryable()
                                         .OrderByDescending(n => n.CreateAt)
                                         .Select(n => new AlarmConfigItem
                                         {
                                             Id = n.Id.ToString(),
                                             Name = n.Name,
                                             Type = n.Type,
                                             BotUrl = n.BotUrl,
                                             CreateAt = n.CreateAt
                                         })
                                         .ToList();
            return Ok(configs);
        }

        [HttpGet("configs/{id}/check")]
        public async Task<IActionResult> CheckConfig([FromRoute] string id)
        {
            var config = _mongoDbContext.Collection<AlarmConfig>().AsQueryable().SingleOrDefault(n => n.Id == new ObjectId(id));
            if (config == null)
            {
                return BadRequest();
            }

            await _larkService.SendTestMsg(config.BotUrl, $"[{config.Name}]的告警标题", $"[{config.Name}]的告警内容");

            return Ok();
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
                new KeyValueItem<string, string>
                { 
                    Key = "status", 
                    Value = "status" 
                }
            };

            return Ok(options);
        }

        [HttpGet("option/operators")]
        public IActionResult GetOperatorOptions()
        {
            var options = new List<KeyValueItem<string, string>>()
            {
                new KeyValueItem<string, string>
                { 
                    Key = "==", 
                    Value = "==" 
                }
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

