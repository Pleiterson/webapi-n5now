using N5Permissions.Application.Events.Permission;
using N5Permissions.Application.Events.PermissionType;
using N5Permissions.Consumer.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace N5Permissions.Consumer.Services;

public class EventProcessorService
{
    private readonly IElasticSearchService _elastic;

    public EventProcessorService(IElasticSearchService elastic)
    {
        _elastic = elastic;
    }

    public async Task ProcessAsync(string topic, string message)
    {
        switch (topic)
        {
            case "permissions-created":
                var created = JsonSerializer.Deserialize<PermissionCreatedEvent>(message);
                await _elastic.IndexPermissionAsync(new PermissionDocument
                {
                    Id = created.Id,
                    NombreEmpleado = created.NombreEmpleado,
                    ApellidoEmpleado = created.ApellidoEmpleado,
                    TipoPermiso = created.TipoPermiso,
                    FechaPermiso = created.FechaPermiso
                });
                break;

            case "permissions-updated":
                var updated = JsonSerializer.Deserialize<PermissionUpdatedEvent>(message);
                await _elastic.IndexPermissionAsync(new PermissionDocument
                {
                    Id = updated.Id,
                    NombreEmpleado = updated.NombreEmpleado,
                    ApellidoEmpleado = updated.ApellidoEmpleado,
                    TipoPermiso = updated.TipoPermiso,
                    FechaPermiso = updated.FechaPermiso
                });
                break;

            case "permissions-deleted":
                var deleted = JsonSerializer.Deserialize<PermissionDeletedEvent>(message);
                await _elastic.DeletePermissionAsync(deleted.Id);
                break;

            case "permissiontypes-created":
                var ptc = JsonSerializer.Deserialize<PermissionTypeCreatedEvent>(message);
                await _elastic.IndexPermissionTypeAsync(new PermissionTypeDocument
                {
                    Id = ptc.Id,
                    Description = ptc.Description
                });
                break;

            case "permissiontypes-updated":
                var ptu = JsonSerializer.Deserialize<PermissionTypeUpdatedEvent>(message);
                await _elastic.IndexPermissionTypeAsync(new PermissionTypeDocument
                {
                    Id = ptu.Id,
                    Description = ptu.Description
                });
                break;

            case "permissiontypes-deleted":
                var ptd = JsonSerializer.Deserialize<PermissionTypeDeletedEvent>(message);
                await _elastic.DeletePermissionTypeAsync(ptd.Id);
                break;
        }
    }
}
