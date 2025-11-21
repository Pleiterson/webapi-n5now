using MediatR;
using N5Permissions.Application.DTOs;
using N5Permissions.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Queries.Permissions.GetAllPermissions
{
    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<PermissionDto>>
    {
        private readonly IPermissionRepository _repository;

        public GetAllPermissionsQueryHandler(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<PermissionDto>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var list = await _repository.GetAllAsync();
            return list.Select(p => new PermissionDto
            {
                Id = p.Id,
                NombreEmpleado = p.NombreEmpleado,
                ApellidoEmpleado = p.ApellidoEmpleado,
                TipoPermiso = p.TipoPermiso,
                FechaPermiso = p.FechaPermiso
            }).ToList();
        }
    }
}
