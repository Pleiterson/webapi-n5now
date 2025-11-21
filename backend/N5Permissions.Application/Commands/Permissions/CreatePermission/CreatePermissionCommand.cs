using MediatR;
using System;

namespace N5Permissions.Application.Commands.Permissions.CreatePermission
{
    public class CreatePermissionCommand : IRequest<int>
    {
        public string NombreEmpleado { get; set; } = string.Empty;
        public string ApellidoEmpleado { get; set; } = string.Empty;
        public int TipoPermiso { get; set; }
        public DateTime FechaPermiso { get; set; }
    }
}
