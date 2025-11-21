using MediatR;
using System;

namespace N5Permissions.Application.Commands.Permissions.UpdatePermission
{
    public class UpdatePermissionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string ApellidoEmpleado { get; set; } = string.Empty;
        public int TipoPermiso { get; set; }
        public DateTime FechaPermiso { get; set; }
    }
}
