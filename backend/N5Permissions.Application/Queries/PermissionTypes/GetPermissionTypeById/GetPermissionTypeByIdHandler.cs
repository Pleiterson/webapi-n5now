using MediatR;
using N5Permissions.Application.DTOs;
using N5Permissions.Application.Queries;
using N5Permissions.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Queries.PermissionTypes.GetPermissionTypeById
{
    public class GetPermissionTypeByIdHandler : IRequestHandler<GetPermissionTypeByIdQuery, PermissionTypeDto?>
    {
        private readonly IPermissionTypeRepository _repo;

        public GetPermissionTypeByIdHandler(IPermissionTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<PermissionTypeDto?> Handle(GetPermissionTypeByIdQuery request, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
                return null;

            return new PermissionTypeDto
            {
                Id = entity.Id,
                Description = entity.Description
            };
        }
    }
}
