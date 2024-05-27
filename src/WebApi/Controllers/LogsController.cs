using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Mongo;
using WebApi.Mongo.Entities;
using MongoDB.Driver.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Caching.Memory;
using DynamicExpresso;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;
        private readonly ILogger<LogsController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly ILarkService _larkService;

        public LogsController(MongoDbContext mongoDbContext, ILogger<LogsController> logger, IMemoryCache memoryCache, ILarkService larkService)
        {
            _mongoDbContext = mongoDbContext;
            _logger = logger;
            _memoryCache = memoryCache;
            _larkService = larkService;
        }

        [HttpPost("apisix")]
        public async Task<IActionResult> SaveApisixLog()
        {
            using (var reader = new StreamReader(HttpContext.Request.Body))
            {
                var postData = await reader.ReadToEndAsync();
                var requests = JsonConvert.DeserializeObject<List<ApisixLogRequest>>(postData);

                foreach (var request in requests)
                {
                    request.Id = ObjectId.GenerateNewId();
                    if (request.Request.Headers.ContainsKey("authorization"))
                    {
                        var jwt = request.Request.Headers["authorization"].Replace("Bearer ", string.Empty);
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(jwt);
                        request.Request.Jwt = token.Claims.ToDictionary(k => k.Type, v => v.Value);

                        if (request.Request.Jwt.Count > 0)
                        {
                            var projectionList = _memoryCache.Get<List<FieldProjection>>(Program.FieldProjectionListKey);
                            request.Request.Projection = new Dictionary<string, string>();

                            foreach (var projection in projectionList)
                            {
                                if (request.Request.Jwt.ContainsKey(projection.FromKey) &&
                                    projection.Mappings.ContainsKey(request.Request.Jwt[projection.FromKey]))
                                {
                                    request.Request.Projection[projection.Key] = projection.Mappings[request.Request.Jwt[projection.FromKey]];
                                }
                            }
                        }

                        var rules = _mongoDbContext.Collection<AlarmRule>().AsQueryable().ToList();

                        if (rules.Count > 0)
                        {
                            var setting = _memoryCache.Get<BasicSetting>(Program.BasicSettingKey);
                            var target = new Interpreter();
                            foreach (var rule in rules)
                            {
                                var fieldValue = string.Empty;
                                if (rule.Field == "status")
                                {
                                    fieldValue = request.Response.Status.ToString();
                                }

                                var isMatched = target.Eval<bool>($"{fieldValue} {rule.Operator} {rule.Value}");
                                if (isMatched)
                                {
                                    var config = _mongoDbContext.Collection<AlarmConfig>().AsQueryable().SingleOrDefault(n => n.Id == rule.AlarmConfigId);
                                    if (config != null)
                                    {
                                        await _larkService.SendRequestAlarmMsg(config.BotUrl, rule.Title, request, setting);
                                    }
                                }
                            }
                        }
                    }
                }

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

        [HttpGet("apisix")]
        public IActionResult ListApisixLog([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            if (pageIndex == null || pageIndex.Value <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                pageSize = 1;
            }

            var query = _mongoDbContext.Collection<ApisixLogRequest>().AsQueryable();

            var logs = query.OrderByDescending(n => n.StartTime).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            var list = logs.Select(n => new ApisixLogItem
            {
                Id = n.Id.ToString(),
                Method = n.Request.Method,
                Url = n.Request.Url,
                Upstream = n.Upstream,
                Status = n.Response.Status,
                Latency = n.Latency,
                StartTime = DateTimeOffset.FromUnixTimeMilliseconds(n.StartTime).UtcDateTime
            });

            var result = new PagingResult<ApisixLogItem>
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Total = query.Count(),
                Records = list
            };

            return Ok(result);
        }

        [HttpGet("apisix/{id}")]
        public async Task<IActionResult> GetApisixLog([FromRoute] string id)
        {
            var log = await _mongoDbContext.Collection<ApisixLogRequest>().Find(n => n.Id == new ObjectId(id)).SingleOrDefaultAsync();
            var detail = new ApisixLogDetail
            {
                Id = log.Id.ToString(),
                Method = log.Request.Method,
                Url = log.Request.Url,
                Upstream = log.Upstream,
                Status = log.Response.Status,
                Latency = log.Latency,
                ClientIp = log.ClientIp,
                StartTime = DateTimeOffset.FromUnixTimeMilliseconds(log.StartTime).UtcDateTime,
                RequestBody = log.Request.Body,
                RequestHeaders = log.Request.Headers.Select(n => new KeyValueItem<string, string>
                {
                    Key = n.Key,
                    Value = n.Value
                }).OrderBy(n => n.Key).ToList(),
                Jwt = log.Request?.Jwt?.Select(n => new KeyValueItem<string, string>
                {
                    Key = n.Key,
                    Value = n.Value
                }).OrderBy(n => n.Key).ToList(),
                Projection = log.Request?.Projection?.Select(n => new KeyValueItem<string, string>
                {
                    Key = n.Key,
                    Value = n.Value
                }).OrderBy(n => n.Key).ToList(),
                QueryStrings = log.Request.QueryString.Select(n => new KeyValueItem<string, string>
                {
                    Key = n.Key,
                    Value = n.Value
                }).OrderBy(n => n.Key).ToList(),
                ResponseHeaders = log.Response.Headers.Select(n => new KeyValueItem<string, string>
                {
                    Key = n.Key,
                    Value = n.Value
                }).OrderBy(n => n.Key).ToList(),
            };

            if (!string.IsNullOrWhiteSpace(log.Response.Body))
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(log.Response.Body);
                detail.ResponseBody = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
            }

            return Ok(detail);
        }

        [HttpGet("apisix/user/{userId}")]
        public IActionResult GetUserApisixLog([FromRoute] string userId, [FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            var setting = _memoryCache.Get<BasicSetting>(Program.BasicSettingKey);
            if (setting == null || string.IsNullOrWhiteSpace(setting.UserIdField))
            {
                return BadRequest();
            }

            if (pageIndex == null || pageIndex.Value <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                pageSize = 1;
            }

            var query = _mongoDbContext.Collection<ApisixLogRequest>().AsQueryable();

            var logs = query.Where(n => n.Request.Jwt != null)
                            .Where(n => n.Request.Jwt.ContainsKey(setting.UserIdField))
                            .Where(n => n.Request.Jwt[setting.UserIdField] == userId)
                            .OrderByDescending(n => n.StartTime).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            var list = logs.Select(n => new ApisixLogItem
            {
                Id = n.Id.ToString(),
                Method = n.Request.Method,
                Url = n.Request.Url,
                Upstream = n.Upstream,
                Status = n.Response.Status,
                Latency = n.Latency,
                StartTime = DateTimeOffset.FromUnixTimeMilliseconds(n.StartTime).UtcDateTime
            });

            var result = new PagingResult<ApisixLogItem>
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Total = query.Count(),
                Records = list
            };

            return Ok(result);
        }
    }
}

