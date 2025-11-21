using MediatR;

namespace N5Permissions.Application.Commands.PermissionTypes.DeletePermissionType
{
    public class DeletePermissionTypeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
