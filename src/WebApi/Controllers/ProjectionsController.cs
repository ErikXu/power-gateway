using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Mongo.Entities;
using WebApi.Mongo;
using WebApi.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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

            await _mongoDbContext.Collection< FieldProjection>().InsertOneAsync(projection);

            return Ok();
        }
    }
}

