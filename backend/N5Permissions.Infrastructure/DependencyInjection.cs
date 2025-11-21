using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Domain.Repositories;
using N5Permissions.Infrastructure.Messaging;
using N5Permissions.Infrastructure.Persistence;
using N5Permissions.Infrastructure.Repositories;
using N5Permissions.Infrastructure.Settings;
using N5Permissions.Infrastructure.UoW;

namespace N5Permissions.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration,
            string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IPermissionTypeRepository, PermissionTypeRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
            services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

            return services;
        }
    }
}
