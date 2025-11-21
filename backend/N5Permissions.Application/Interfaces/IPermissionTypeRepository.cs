using N5Permissions.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N5Permissions.Application.Interfaces
{
    public interface IPermissionTypeRepository
    {
        Task<PermissionType> AddAsync(PermissionType entity);
        Task<PermissionType?> GetByIdAsync(int id);
        Task<List<PermissionType>> GetAllAsync();
        Task UpdateAsync(PermissionType entity);
        Task DeleteAsync(PermissionType entity);
    }
}
