using N5Permissions.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N5Permissions.Domain.Repositories
{
    public interface IPermissionRepository
    {
        Task<Permission> AddAsync(Permission entity);
        Task<List<Permission>> GetAllAsync();
        Task<Permission?> GetByIdAsync(int id);
        Task UpdateAsync(Permission entity);
        Task DeleteAsync(Permission entity);
    }
}
