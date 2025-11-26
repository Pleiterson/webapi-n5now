using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N5Permissions.API;
using N5Permissions.Infrastructure.Persistence;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Tests.Integration.Fakes;

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
                builder.UseEnvironment("Testing"); // rodando testes!

                builder.ConfigureServices(services =>
                {
                    // 1. Remove AppDbContext existente
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // 2. Remove provider SQL Server
                    foreach (var s in services.Where(s =>
                        s.ImplementationType?.Namespace?.Contains("SqlServer") == true).ToList())
                    {
                        services.Remove(s);
                    }

                    // Remove Kafka real
                    var kafkaDescriptor = services.SingleOrDefault(s =>
                        s.ServiceType == typeof(IKafkaProducerService));

                    if (kafkaDescriptor != null)
                        services.Remove(kafkaDescriptor);

                    // Adiciona Fake Kafka
                    services.AddSingleton<IKafkaProducerService, FakeKafkaProducerService>();

                    // 3. Adiciona SQLite
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite(_connection));

                    // 4. Cria banco
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
