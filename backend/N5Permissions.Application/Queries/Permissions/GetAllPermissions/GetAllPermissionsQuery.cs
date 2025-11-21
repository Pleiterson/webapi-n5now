using MediatR;
using N5Permissions.Application.DTOs;
using System.Collections.Generic;

namespace N5Permissions.Application.Queries.Permissions.GetAllPermissions
{
    public class GetAllPermissionsQuery : IRequest<List<PermissionDto>> { }
}
