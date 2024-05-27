
using dotenv.net;
using WebApi.HostedServices;
using WebApi.Mongo;
using WebApi.Services;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotEnv.Load(options: new DotEnvOptions(overwriteExistingVars: false));

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMemoryCache();

            builder.Services.Add(ServiceDescriptor.Singleton(new MongoDbContext(Environment.GetEnvironmentVariable("MONGO_CONNECTION"), Environment.GetEnvironmentVariable("MONGO_DBNAME"))));

            builder.Services.AddSingleton<ILarkService, LarkService>();

            builder.Services.AddHostedService<CalculateService>();
            builder.Services.AddHostedService<RefreshCacheService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        public static string FieldProjectionListKey { get; } = "Cache_FieldProjectionList";

        public static string BasicSettingKey { get; } = "Cache_BasicSetting";
    }
}

