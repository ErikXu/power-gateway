using Microsoft.AspNetCore.Mvc;
using WebApi.Mongo.Entities;
using WebApi.Mongo;
using WebApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectionsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;

        public ProjectionsController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpPost]
        public async Task <IActionResult> Create([FromBody] FieldProjectionForm form)
        {
            var projection = new FieldProjection
            {
                Name = form.Name,
                Key = form.Key,
                FromKey = form.FromKey,
                Mappings = form.Mappings
            };

            await _mongoDbContext.Collection<FieldProjection>().InsertOneAsync(projection);

            return Ok();
        }

        [HttpGet]
        public IActionResult List()
        {
            var list = _mongoDbContext.Collection<FieldProjection>()
                                         .AsQueryable()
                                         .OrderByDescending(n => n.CreateAt)
                                         .ToList()
                                         .Select(n => new FieldProjectionItem
                                         {
                                             Id = n.Id.ToString(),
                                             Name = n.Name,
                                             Key = n.Key,
                                             FromKey = n.FromKey,
                                             Mappings = n.Mappings,
                                             MappingText = JsonConvert.SerializeObject(n.Mappings, Formatting.Indented),
                                             CreateAt = n.CreateAt
                                         })
                                         .ToList();
            return Ok(list);
        }
    }
}

