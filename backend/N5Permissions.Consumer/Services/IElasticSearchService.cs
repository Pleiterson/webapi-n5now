using System.Threading.Tasks;
using N5Permissions.Consumer.Models;

namespace N5Permissions.Consumer.Services;

public interface IElasticSearchService
{
    Task IndexPermissionAsync(PermissionDocument doc);
    Task IndexPermissionTypeAsync(PermissionTypeDocument doc);
    Task DeletePermissionAsync(int id);
    Task DeletePermissionTypeAsync(int id);
}
