using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Mongo.Entities;
using WebApi.Mongo;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BehaviorsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;

        public BehaviorsController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpPost("mappings")]
        public async Task<IActionResult> CreateMapping([FromBody] BehaviorMappingForm form)
        {
            var mapping = new BehaviorMapping
            {
                Method = form.Method,
                Operator = form.Operator,
                Value = form.Value
            };

            await _mongoDbContext.Collection<BehaviorMapping>().InsertOneAsync(mapping);

            return Ok();
        }

        [HttpGet("option/methods")]
        public IActionResult GetMethodOptions()
        {
            var options = new List<KeyValueItem<string, string>>()
            {
                new KeyValueItem<string, string>
                { 
                    Key = "GET", 
                    Value = "GET" 
                },
                new KeyValueItem<string, string>
                { 
                    Key = "POST", 
                    Value = "POST" 
                },
                new KeyValueItem<string, string>
                {
                    Key = "PUT",
                    Value = "PUT"
                },
                new KeyValueItem<string, string>
                {
                    Key = "DELETE",
                    Value = "DELETE"
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
                    Value = "equals"
                },
                new KeyValueItem<string, string>
                {
                    Key = "startWith",
                    Value = "startWith"
                },
                new KeyValueItem<string, string>
                {
                    Key = "regex",
                    Value = "regex"
                }
            };

            return Ok(options);
        }
    }
}
