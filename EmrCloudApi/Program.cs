using EmrCloudApi.Configs.Dependency;
using EmrCloudApi.Configs.Options;
using EmrCloudApi.Realtime;
using EmrCloudApi.Security;
using Infrastructure.Logger;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using PostgreDataContext;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEmrOptions(builder.Configuration);
builder.Services.AddMemoryCache();
///builder.Services.AddResponseCaching();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

int minWorker, minIOC;
ThreadPool.GetMinThreads(out minWorker, out minIOC);
if (ThreadPool.SetMinThreads(3000, minIOC))
{
    Console.WriteLine("Set Min thread");
}
else
{
    Console.WriteLine("Not set min thread");
}

#if DEBUG
//Temp drop to customer test
builder.Services.AddSignalR().AddMessagePackProtocol()
.AddStackExchangeRedis(o =>
        {
            o.ConnectionFactory = async writer =>
            {
                var config = new ConfigurationOptions
                {
                    AbortOnConnectFail = false
                };

                string redisHost = builder.Configuration["RedisHostLocal"] ?? string.Empty;
                string redisPort = builder.Configuration["RedisPort"] ?? string.Empty;
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
                    Console.WriteLine("Connection to Redis failed." + Environment.NewLine + e.Exception.Message + Environment.NewLine + e.Exception.StackTrace);
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // allow credentials
    });
});

// Authentication
builder.Services.SetupAuthentication(builder.Configuration);

//Serilog 
builder.Host.UseSerilog((ctx, lc) => lc
       .WriteTo.Console()
       .ReadFrom.Configuration(ctx.Configuration));

var dependencySetup = new ModuleDependencySetup();
dependencySetup.Run(builder.Services);

// Migration for AdminDBContext
// This config is needed for EF Core Migrations to find the DbContext
builder.Services.AddDbContext<AdminDataContext>(options =>
{
    var connectionStr = builder.Configuration["AdminDatabase"];
    options.UseNpgsql(connectionStr, b => b.MigrationsAssembly("EmrCloudApi"));
});

var app = builder.Build();

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AdminDataContext>();

    // Here is the migration executed
    //dbContext.Database.Migrate();
}

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
var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("env.json", false, true)
                .AddJsonFile($"env.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true,
                    true)
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Debug()
    .CreateLogger();

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { context.Request?.Headers?["Origin"].ToString() ?? string.Empty });
        context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" });
        context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
        context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
        context.Response.Headers.Add("Access-Control-Max-Age", "7200");
        context.Response.Headers.Add("Access-Control-Allow-Login-Key", new[] { context.Request?.Headers?["Login-Key"].ToString() ?? string.Empty });
        context.Response.StatusCode = 200;
        await next(context);
    }
    else
    {
        context.Request.EnableBuffering();

        using (var loggingHandler = context.RequestServices.GetService<ILoggingHandler>())
        {
            try
            {
                await loggingHandler!.WriteLogStartAsync("Start request");
                context.Response.Headers.Add("Access-Control-Allow-Login-Key", new[] { context.Request?.Headers?["Login-Key"].ToString() ?? string.Empty });
                context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" }); // allow credentials
                context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept" }); // allow header
                context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" }); // allow methods
                await next(context);
            }
            catch (Exception ex)
            {
                await loggingHandler!.WriteLogExceptionAsync(ex);
            }
            finally
            {
                await loggingHandler!.WriteLogEndAsync("End request");
            }
        }
    }
});

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

///app.UseResponseCaching();

app.UseResponseCompression();

// SignalR Hub
app.MapHub<CommonHub>("/CommonHub");

//Serilog 
app.UseSerilogRequestLogging();

app.Run();
