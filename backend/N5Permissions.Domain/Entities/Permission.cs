using System;

namespace N5Permissions.Domain.Entities
{
    public class Permission
    {
        public int Id { get; private set; }
        public string NombreEmpleado { get; private set; } = string.Empty;
        public string ApellidoEmpleado { get; private set; } = string.Empty;
        public int TipoPermiso { get; private set; }
        public DateTime FechaPermiso { get; private set; }

        public PermissionType PermissionType { get; private set; } = null!;

        public Permission(
            string nombreEmpleado,
            string apellidoEmpleado,
            int tipoPermiso,
            DateTime fechaPermiso)
        {
            SetNombre(nombreEmpleado);
            SetApellido(apellidoEmpleado);
            SetTipoPermiso(tipoPermiso);
            SetFecha(fechaPermiso);
        }

        public void SetNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Nome inválido!");

            NombreEmpleado = nombre;
        }

        public void SetApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("Sobrenome inválido!");

            ApellidoEmpleado = apellido;
        }

        public void SetTipoPermiso(int tipoPermiso)
        {
            if (tipoPermiso <= 0)
                throw new ArgumentException("Tipo de permissão inválido!");

            TipoPermiso = tipoPermiso;
        }

        public void SetFecha(DateTime fecha)
        {
            if (fecha == default)
                throw new ArgumentException("Data inválida!");

            FechaPermiso = fecha;
        }
    }
}
