using EmrCalculateApi;
using EmrCalculateApi.Futan.ViewModels;
using EmrCalculateApi.Ika.ViewModels;
using EmrCalculateApi.Implementation;
using EmrCalculateApi.Interface;
using EmrCalculateApi.ReceFutan.ViewModels;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var services = builder.Services;

services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<ITenantProvider, TenantProvider>();
services.AddScoped<ISystemConfigProvider, SystemConfigProvider>();
services.AddScoped<IEmrLogger, EmrLogger>();

services.AddScoped<IFutancalcViewModel, FutancalcViewModel>();
services.AddScoped<IReceFutanViewModel, ReceFutanViewModel>();
services.AddScoped<IIkaCalculateViewModel, IkaCalculateViewModel>();

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

app.UseMiddleware<HttpsRedirectMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
