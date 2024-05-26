
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using WebApi.Mongo;
using WebApi.Mongo.Entities;

namespace WebApi.HostedServices
{
    public class CalculateService : IHostedService, IDisposable
    {
        private Timer _timer;

        private readonly ILogger<CalculateService> _logger;
        private readonly MongoDbContext _mongoDbContext;

        public CalculateService(ILogger<CalculateService> logger, MongoDbContext mongoDbContext)
        {
            _logger = logger;
            _mongoDbContext = mongoDbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Calculate hosted service] is running...");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var basicSetting = _mongoDbContext.Collection<BasicSetting>().AsQueryable().FirstOrDefault(n => n.Id == ObjectId.Empty);
            if (basicSetting == null)
            {
                return;
            }

            var now = DateTime.UtcNow;

            CalculateQps1Munite(now, basicSetting);
            CalculateQps1Hour(now, basicSetting);
            CalculateQps1Day(now, basicSetting);

            CalculateIpRequest1Munite(now);
            CalculateIpRequest1Hour(now);
        }

        private void CalculateQps1Munite(DateTime now, BasicSetting basicSetting)
        {
            var endTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc);

            var qps1Munite = _mongoDbContext.Collection<Qps1Munite>().AsQueryable().SingleOrDefault(n => n.Time == endTime);
            if (qps1Munite != null)
            {
                return;
            }

            var startTime = endTime.AddMinutes(-1);

            var endTimestamp = new DateTimeOffset(endTime).Millisecond;
            var startTimestamp = new DateTimeOffset(startTime).Millisecond;

            var logs = _mongoDbContext.Collection<ApisixLogRequest>().AsQueryable()
                                      .Where(n => n.StartTime >= endTimestamp && n.StartTime < startTimestamp)
                                      .Select(n => new { n.Latency, n.StartTime })
                                      .ToList();

            if (logs.Count <= 0)
            {
                qps1Munite = new Qps1Munite
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("HH:mm"),
                    Total = 0,
                    Value = 0,
                    LatencyBenchmark = basicSetting.Latency,
                    LowPerformanceCount = 0,
                    LatencyAvg = 0,
                    LatencyMax = 0,
                    LatencyMin = 0
                };
            }
            else
            {
                qps1Munite = new Qps1Munite
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("HH:mm"),
                    Total = logs.Count,
                    Value = logs.Count / 60,
                    LatencyBenchmark = basicSetting.Latency,
                    LowPerformanceCount = logs.Where(n => n.Latency > basicSetting.Latency).Count(),
                    LatencyAvg = logs.Sum(n => n.Latency) / logs.Count,
                    LatencyMin = logs.Min(n => n.Latency),
                    LatencyMax = logs.Max(n => n.Latency)
                };
            }

            _mongoDbContext.Collection<Qps1Munite>().InsertOne(qps1Munite);
        }

        private void CalculateQps1Hour(DateTime now, BasicSetting basicSetting)
        {
            var endTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);

            var qps1Hour = _mongoDbContext.Collection<Qps1Hour>().AsQueryable().SingleOrDefault(n => n.Time == endTime);
            if (qps1Hour != null)
            {
                return;
            }

            var startTime = endTime.AddHours(-1);

            var endTimestamp = new DateTimeOffset(endTime).Millisecond;
            var startTimestamp = new DateTimeOffset(startTime).Millisecond;

            var logs = _mongoDbContext.Collection<ApisixLogRequest>().AsQueryable()
                                      .Where(n => n.StartTime >= endTimestamp && n.StartTime < startTimestamp)
                                      .Select(n => new { n.Latency, n.StartTime })
                                      .ToList();

            if (logs.Count <= 0)
            {
                qps1Hour = new Qps1Hour
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("MM-dd HH"),
                    Total = 0,
                    Value = 0,
                    LatencyBenchmark = basicSetting.Latency,
                    LowPerformanceCount = 0,
                    LatencyAvg = 0,
                    LatencyMax = 0,
                    LatencyMin = 0
                };
            }
            else
            {
                qps1Hour = new Qps1Hour
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("MM-dd HH"),
                    Total = logs.Count,
                    Value = logs.Count / 60 / 60,
                    LatencyBenchmark = basicSetting.Latency,
                    LowPerformanceCount = logs.Where(n => n.Latency > basicSetting.Latency).Count(),
                    LatencyAvg = logs.Sum(n => n.Latency) / logs.Count,
                    LatencyMin = logs.Min(n => n.Latency),
                    LatencyMax = logs.Max(n => n.Latency)
                };
            }

            _mongoDbContext.Collection<Qps1Hour>().InsertOne(qps1Hour);
        }

        private void CalculateQps1Day(DateTime now, BasicSetting basicSetting)
        {
            var endTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

            var qps1Day = _mongoDbContext.Collection<Qps1Day>().AsQueryable().SingleOrDefault(n => n.Time == endTime);
            if (qps1Day != null)
            {
                return;
            }

            var startTime = endTime.AddHours(-24);

            var endTimestamp = new DateTimeOffset(endTime).Millisecond;
            var startTimestamp = new DateTimeOffset(startTime).Millisecond;

            var logs = _mongoDbContext.Collection<ApisixLogRequest>().AsQueryable()
                                      .Where(n => n.StartTime >= endTimestamp && n.StartTime < startTimestamp)
                                      .Select(n => new { n.Latency, n.StartTime })
                                      .ToList();

            if (logs.Count <= 0)
            {
                qps1Day = new Qps1Day
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("yyyy-MM-dd"),
                    Total = 0,
                    Value = 0,
                    LatencyBenchmark = basicSetting.Latency,
                    LowPerformanceCount = 0,
                    LatencyAvg = 0,
                    LatencyMax = 0,
                    LatencyMin = 0
                };
            }
            else
            {
                qps1Day = new Qps1Day
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("yyyy-MM-dd"),
                    Total = logs.Count,
                    Value = logs.Count / 24 / 60 / 60,
                    LatencyBenchmark = basicSetting.Latency,
                    LowPerformanceCount = logs.Where(n => n.Latency > basicSetting.Latency).Count(),
                    LatencyAvg = logs.Sum(n => n.Latency) / logs.Count,
                    LatencyMin = logs.Min(n => n.Latency),
                    LatencyMax = logs.Max(n => n.Latency)
                };
            }

            _mongoDbContext.Collection<Qps1Day>().InsertOne(qps1Day);
        }

        private void CalculateIpRequest1Munite(DateTime now)
        {
            var endTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Utc);
            var startTime = endTime.AddMinutes(-1);

            var endTimestamp = new DateTimeOffset(endTime).Millisecond;
            var startTimestamp = new DateTimeOffset(startTime).Millisecond;

            var logs = _mongoDbContext.Collection<ApisixLogRequest>().AsQueryable()
                                      .Where(n => n.StartTime >= endTimestamp && n.StartTime < startTimestamp)
                                      .Select(n => new { n.ClientIp, n.StartTime })
                                      .ToList();

            if (logs.Count <= 0)
            {
                return;
            }
            else
            {
                var dic = new Dictionary<string, int>();
                foreach (var log in logs)
                {
                    if (dic.ContainsKey(log.ClientIp))
                    {
                        dic[log.ClientIp] = dic[log.ClientIp] + 1;
                    }
                    else
                    {
                        dic[log.ClientIp] = 1;
                    }
                }

                var ipRequest1Munite = dic.Select(n => new IpRequest1Munite
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("HH:mm"),
                    Ip = n.Key,
                    Count = n.Value,
                    CreateAt = DateTime.UtcNow
                }).ToList();

                _mongoDbContext.Collection<IpRequest1Munite>().InsertMany(ipRequest1Munite);
            }
        }

        private void CalculateIpRequest1Hour(DateTime now)
        {
            var endTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);
            var startTime = endTime.AddHours(-1);

            var endTimestamp = new DateTimeOffset(endTime).Millisecond;
            var startTimestamp = new DateTimeOffset(startTime).Millisecond;


            var logs = _mongoDbContext.Collection<ApisixLogRequest>().AsQueryable()
                                      .Where(n => n.StartTime >= endTimestamp && n.StartTime < startTimestamp)
                                      .Select(n => new { n.ClientIp, n.StartTime })
                                      .ToList();

            if (logs.Count <= 0)
            {
                return;
            }
            else
            {
                var dic = new Dictionary<string, int>();
                foreach (var log in logs)
                {
                    if (dic.ContainsKey(log.ClientIp))
                    {
                        dic[log.ClientIp] = dic[log.ClientIp] + 1;
                    }
                    else
                    {
                        dic[log.ClientIp] = 1;
                    }
                }

                var ipRequest1Hour = dic.Select(n => new IpRequest1Hour
                {
                    Time = endTime,
                    Text = endTime.ToLocalTime().ToString("MM-dd HH"),
                    Ip = n.Key,
                    Count = n.Value,
                    CreateAt = DateTime.UtcNow
                }).ToList();

                _mongoDbContext.Collection<IpRequest1Hour>().InsertMany(ipRequest1Hour);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[Calculate hosted service] is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

