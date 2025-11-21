using MediatR;
using N5Permissions.Application.DTOs;

namespace N5Permissions.Application.Commands.PermissionTypes.UpdatePermissionType
{
    public class UpdatePermissionTypeCommand : IRequest<PermissionTypeDto>
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
