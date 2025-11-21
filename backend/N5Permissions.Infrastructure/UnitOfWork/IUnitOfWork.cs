using N5Permissions.Domain.Repositories;
using System.Threading.Tasks;

namespace N5Permissions.Infrastructure.UoW
{
    public interface IUnitOfWork
    {
        IPermissionRepository Permissions { get; }
        IPermissionTypeRepository PermissionTypes { get; }
        Task<int> SaveChangesAsync();
    }
}
