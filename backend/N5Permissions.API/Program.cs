using MediatR;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using N5Permissions.Application;
using N5Permissions.Infrastructure;
using N5Permissions.Infrastructure.Persistence;
using N5Permissions.Application.Commands.Permissions.CreatePermission;

var builder = WebApplication.CreateBuilder(args);

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreatePermissionCommandHandler).Assembly));

// Infrastructure
builder.Services.AddInfrastructure(
    builder.Configuration,
    builder.Configuration.GetConnectionString("DefaultConnection")!
);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// rodando testes?
var isTesting = builder.Environment.EnvironmentName == "Testing";

if (!isTesting)
{
    // DbContext
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Run seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

public partial class Program { }
