using MediatR;
using N5Permissions.Application.DTOs;

namespace N5Permissions.Application.Commands.PermissionTypes.CreatePermissionType
{
    public class CreatePermissionTypeCommand : IRequest<PermissionTypeDto>
    {
        public string Description { get; set; } = string.Empty;
    }
}
