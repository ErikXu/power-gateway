using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Models;
using WebApi.Mongo;
using WebApi.Mongo.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly MongoDbContext _mongoDbContext;

        public SettingsController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var setting = _mongoDbContext.Collection<BasicSetting>().AsQueryable().FirstOrDefault();
            if (setting == null)
            {
                setting = new BasicSetting
                {
                    Id = ObjectId.Empty,
                    Latency = 500,
                    UserIdField = "userId",
                    Endpoint = Request.GetEncodedUrl().Replace(Request.GetEncodedPathAndQuery(), string.Empty),
                    DataKeepDays = 30,
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                };

                await _mongoDbContext.Collection<BasicSetting>().InsertOneAsync(setting);
            }

            return Ok(setting);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] BasicSettingForm form)
        {
            var setting = _mongoDbContext.Collection<BasicSetting>().AsQueryable().FirstOrDefault();
            if (setting == null)
            {
                setting = new BasicSetting
                {
                    Id = ObjectId.Empty,
                    Latency = form.Latency,
                    UserIdField = form.UserIdField,
                    Endpoint = form.Endpoint,
                    DataKeepDays = form.DataKeepDays,
                    CreateAt = DateTime.UtcNow,
                    UpdateAt = DateTime.UtcNow
                };

                await _mongoDbContext.Collection<BasicSetting>().InsertOneAsync(setting);
            }
            else
            {
                setting.Latency = form.Latency;
                setting.UserIdField = form.UserIdField;
                setting.Endpoint = form.Endpoint;
                setting.DataKeepDays = form.DataKeepDays;
                setting.UpdateAt = DateTime.UtcNow;

                await _mongoDbContext.Collection<BasicSetting>().ReplaceOneAsync(n => n.Id == setting.Id, setting);
            }

            return Ok();
        }
    }
}
