using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Mongo;
using WebApi.Mongo.Entities;
using MongoDB.Driver.Linq;
using DnsClient.Protocol;
using System.IdentityModel.Tokens.Jwt;

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

                foreach (var request in requests)
                {
                    if (request.Request.Headers.ContainsKey("authorization"))
                    {
                        var jwt = request.Request.Headers["authorization"].Replace("Bearer ", string.Empty);
                        var handler = new JwtSecurityTokenHandler();
                        var token = handler.ReadJwtToken(jwt);
                        request.Request.Jwt = token.Claims.ToDictionary(k => k.Type, v => v.Value);
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

            var logs = query.OrderBy(n => n.StartTime).OrderByDescending(n => n.Id).Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

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
        public async Task<IActionResult> GetApisixLog(string id)
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
    }
}
