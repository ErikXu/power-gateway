using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Mongo.Entities;
using WebApi.Mongo;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;

        public RequestsController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpGet("ip/1m")]
        public IActionResult ListIpRequest1Munite([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            if (pageIndex == null || pageIndex.Value <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                pageSize = 1;
            }

            var query = _mongoDbContext.Collection<IpRequest1Munite>().AsQueryable();

            var logs = query.OrderByDescending(n => n.Time)
                            .ThenByDescending(n => n.Count)
                            .Skip((pageIndex.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value)
                            .ToList();

            var result = new PagingResult<IpRequest1Munite>
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Total = query.Count(),
                Records = logs
            };

            return Ok(result);
        }

        [HttpGet("ip/1h")]
        public IActionResult ListIpRequest1Hour([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            if (pageIndex == null || pageIndex.Value <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                pageSize = 1;
            }

            var query = _mongoDbContext.Collection<IpRequest1Hour>().AsQueryable();

            var logs = query.OrderByDescending(n => n.Time)
                            .ThenByDescending(n => n.Count)
                            .Skip((pageIndex.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value)
                            .ToList();

            var result = new PagingResult<IpRequest1Hour>
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Total = query.Count(),
                Records = logs
            };

            return Ok(result);
        }

        [HttpGet("user/1m")]
        public IActionResult ListUserRequest1Munite([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            if (pageIndex == null || pageIndex.Value <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                pageSize = 1;
            }

            var query = _mongoDbContext.Collection<UserRequest1Munite>().AsQueryable();

            var logs = query.OrderByDescending(n => n.Time)
                            .ThenByDescending(n => n.Count)
                            .Skip((pageIndex.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value)
                            .ToList();

            var result = new PagingResult<UserRequest1Munite>
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Total = query.Count(),
                Records = logs
            };

            return Ok(result);
        }

        [HttpGet("user/1h")]
        public IActionResult ListUserRequest1Hour([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
        {
            if (pageIndex == null || pageIndex.Value <= 0)
            {
                pageIndex = 1;
            }

            if (pageSize == null || pageSize.Value <= 0)
            {
                pageSize = 1;
            }

            var query = _mongoDbContext.Collection<UserRequest1Hour>().AsQueryable();

            var logs = query.OrderByDescending(n => n.Time)
                            .ThenByDescending(n => n.Count)
                            .Skip((pageIndex.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value)
                            .ToList();

            var result = new PagingResult<UserRequest1Hour>
            {
                PageIndex = pageIndex.Value,
                PageSize = pageSize.Value,
                Total = query.Count(),
                Records = logs
            };

            return Ok(result);
        }
    }
}
