using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

var builder = WebApplication.CreateBuilder(args);

// This config is needed for EF Core Migrations to find the DbContext
builder.Services.AddDbContext<TenantDataContext>(options =>
{
    var connectionStr = builder.Configuration["TenantDbSample"];
    options.UseNpgsql(connectionStr, b => b.MigrationsAssembly("TenantMigration"));
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
