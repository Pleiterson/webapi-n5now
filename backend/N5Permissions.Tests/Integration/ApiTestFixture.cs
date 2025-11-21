using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N5Permissions.API;
using N5Permissions.Infrastructure.Persistence;

namespace N5Permissions.Tests.Integration;

public class ApiTestFixture : IDisposable
{
    public readonly HttpClient Client;
    private readonly SqliteConnection _connection;
    private readonly WebApplicationFactory<Program> _factory;

    public ApiTestFixture()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove DB real
                    var descriptor = services.SingleOrDefault(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Adiciona SQLite em memória
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite(_connection));

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();
                });
            });

        Client = _factory.CreateClient();
    }

    public void Dispose()
    {
        _connection.Close();
    }
}
