using System;

namespace N5Permissions.Application.Events.Permission
{
    public class PermissionCreatedEvent
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public string ApellidoEmpleado { get; set; } = string.Empty;
        public int TipoPermiso { get; set; }
        public DateTime FechaPermiso { get; set; }
    }
}
