using N5Permissions.Infrastructure.Persistence;
using N5Permissions.Infrastructure.Repositories;
using N5Permissions.Domain.Repositories;
using System.Threading.Tasks;

namespace N5Permissions.Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IPermissionRepository Permissions { get; }
        public IPermissionTypeRepository PermissionTypes { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Permissions = new PermissionRepository(context);
            PermissionTypes = new PermissionTypeRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
