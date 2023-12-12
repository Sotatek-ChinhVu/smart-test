using Interactor.Realtime;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using SuperAdmin.Configs.Dependency;
using SuperAdmin.Configs.Options;
using SuperAdminAPI.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEmrOptions(builder.Configuration);

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

// Add services to the container.
#if DEBUG
builder.Services.AddSignalR();
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

builder.Services.AddControllers();
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
            .AllowAnyMethod();
    });
});
// Authentication
builder.Services.SetupAuthentication(builder.Configuration);
var dependencySetup = new ModuleDependencySetup();
dependencySetup.Run(builder.Services);

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
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// SignalR Hub
app.MapHub<CommonHub>("/CommonHub");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Template")),
    RequestPath = "/Template"
});

app.Run();

