using MediatR;
using N5Permissions.Application.DTOs;
using System.Collections.Generic;

namespace N5Permissions.Application.Queries.PermissionTypes.GetAllPermissionTypes
{
    public class GetAllPermissionTypesQuery : IRequest<List<PermissionTypeDto>>
    {
    }
}
