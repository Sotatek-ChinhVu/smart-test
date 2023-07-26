using EmrCalculateApi.Futan.ViewModels;
using EmrCalculateApi.Ika.ViewModels;
using EmrCalculateApi.Implementation;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Realtime;
using EmrCalculateApi.ReceFutan.ViewModels;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

#if DEBUG
builder.Services.AddSignalR()
        .AddStackExchangeRedis(o =>
        {
            o.ConnectionFactory = async writer =>
            {
                var config = new ConfigurationOptions
                {
                    AbortOnConnectFail = false
                };

                string redisHost = builder.Configuration["RedisHostLocal"];
                string redisPort = builder.Configuration["RedisPort"];
                config.EndPoints.Add(redisHost, int.Parse(redisPort));

                var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                connection.ConnectionFailed += (_, e) =>
                {
                    Console.WriteLine("Connection to Redis failed.");
                };

                if (!connection.IsConnected)
                {
                    Console.WriteLine("Did not connect to Redis.");
                }
                else
                {
                    Console.WriteLine("Connected to Redis.");
                }

                return connection;
            };
        });
#else
// Setup signalR
builder.Services.AddSignalR()
        .AddStackExchangeRedis(o =>
        {
            o.ConnectionFactory = async writer =>
            {
                var config = new ConfigurationOptions
                {
                    AbortOnConnectFail = false
                };

                string redisHost = builder.Configuration["RedisHost"];
                string redisPort = builder.Configuration["RedisPort"];
                config.EndPoints.Add(redisHost, int.Parse(redisPort));

                var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                connection.ConnectionFailed += (_, e) =>
                {
                    Console.WriteLine("Connection to Redis failed.");
                };

                if (!connection.IsConnected)
                {
                    Console.WriteLine("Did not connect to Redis.");
                }
                else
                {
                    Console.WriteLine("Connected to Redis.");
                }

                return connection;
            };
        });
#endif

var services = builder.Services;

services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<ITenantProvider, TenantProvider>();
services.AddScoped<ISystemConfigProvider, SystemConfigProvider>();
services.AddScoped<IEmrLogger, EmrLogger>();

services.AddScoped<IFutancalcViewModel, FutancalcViewModel>();
services.AddScoped<IReceFutanViewModel, ReceFutanViewModel>();
services.AddScoped<IIkaCalculateViewModel, IkaCalculateViewModel>();
services.AddTransient<IWebSocketService, WebSocketService>();

//Serilog 
builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

//Add config from json file
string enviroment = "Development";
if (app.Environment.IsProduction() ||
    app.Environment.IsStaging())
{
    enviroment = "Staging";
}
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddJsonFile("env.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"env.{enviroment}.json", true, true);
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() ||
    app.Environment.IsProduction() ||
    app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// serilog
var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("env.json", false, true)
                .AddJsonFile($"env.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true,
                    true)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(Configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// SignalR Hub
app.MapHub<CommonHub>("/CommonHub");

//Serilog 
app.UseSerilogRequestLogging();

app.Run();
