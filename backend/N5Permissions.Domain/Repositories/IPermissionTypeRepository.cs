using N5Permissions.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N5Permissions.Domain.Repositories
{
    public interface IPermissionTypeRepository
    {
        Task<PermissionType?> GetByIdAsync(int id);
        Task<List<PermissionType>> GetAllAsync();
        Task<PermissionType> AddAsync(PermissionType entity);
        Task UpdateAsync(PermissionType entity);
        Task DeleteAsync(PermissionType entity);
    }
}
