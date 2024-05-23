using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using WebApi.Mongo;
using WebApi.Mongo.Entities;

namespace WebApi.HostedServices
{
    public class RefreshCacheService : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly ILogger<RefreshCacheService> _logger;
        private readonly MongoDbContext _mongoDbContext;
        private readonly IMemoryCache _memoryCache;

        public RefreshCacheService(ILogger<RefreshCacheService> logger, MongoDbContext mongoDbContext, IMemoryCache memoryCache)
        {
            _logger = logger;
            _mongoDbContext = mongoDbContext;
            _memoryCache = memoryCache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Refresh cache hosted service] is running...");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var projectionList = _mongoDbContext.Collection<FieldProjection>().AsQueryable().ToList();
            _memoryCache.Set(Program.FieldProjectionListKey, projectionList);
            _logger.LogInformation("[Refresh cache hosted service] Projection cache refreshed.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Refresh cache hosted service] is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

