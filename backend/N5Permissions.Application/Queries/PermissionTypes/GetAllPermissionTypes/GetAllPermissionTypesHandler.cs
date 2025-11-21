using MediatR;
using N5Permissions.Application.DTOs;
using N5Permissions.Application.Queries;
using N5Permissions.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Queries.PermissionTypes.GetAllPermissionTypes
{
    public class GetAllPermissionTypesHandler : IRequestHandler<GetAllPermissionTypesQuery, List<PermissionTypeDto>>
    {
        private readonly IPermissionTypeRepository _repo;

        public GetAllPermissionTypesHandler(IPermissionTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<PermissionTypeDto>> Handle(GetAllPermissionTypesQuery request, CancellationToken ct)
        {
            var list = await _repo.GetAllAsync();
            return list.Select(x => new PermissionTypeDto
            {
                Id = x.Id,
                Description = x.Description
            }).ToList();
        }
    }
}
