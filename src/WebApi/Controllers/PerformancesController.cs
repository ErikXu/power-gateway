using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApi.Models;
using WebApi.Mongo;
using WebApi.Mongo.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformancesController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;

        public PerformancesController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpGet("qps/1m")]
        public IActionResult GetQps1Munite()
        {
            var qpsList = _mongoDbContext.Collection<Qps1Munite>().AsQueryable().OrderByDescending(n => n.Time).ToList();

            return Ok(qpsList);
        }
    }
}

