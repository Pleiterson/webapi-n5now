using MediatR;
using N5Permissions.Application.DTOs;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Queries.Permissions.GetPermissionById
{
    public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, PermissionDto?>
    {
        private readonly IPermissionRepository _repository;

        public GetPermissionByIdQueryHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PermissionDto?> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            var p = await _repository.GetByIdAsync(request.Id);
            if (p == null) return null;

            return new PermissionDto
            {
                Id = p.Id,
                NombreEmpleado = p.NombreEmpleado,
                ApellidoEmpleado = p.ApellidoEmpleado,
                TipoPermiso = p.TipoPermiso,
                FechaPermiso = p.FechaPermiso
            };
        }
    }
}
