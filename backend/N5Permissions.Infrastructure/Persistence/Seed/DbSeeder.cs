using Microsoft.EntityFrameworkCore;
using N5Permissions.Domain.Entities;
using System.Threading.Tasks;

namespace N5Permissions.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!context.PermissionTypes.Any())
            {
                context.PermissionTypes.AddRange(
                    new PermissionType("Read"),
                    new PermissionType("Write"),
                    new PermissionType("Execute"),
                    new PermissionType("Delete")
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
