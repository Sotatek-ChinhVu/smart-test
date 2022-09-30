using EmrCalculateApi.Futan;
using EmrCalculateApi.Implementation;
using EmrCalculateApi.Implementation.IkaCalculate;
using EmrCalculateApi.Interface;
using Infrastructure.Common;
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

services.AddScoped<IFutanCalculate, FutanCalculate>();
services.AddScoped<IIkaCalculate, IkaCalculate>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
