using MediatR;

namespace N5Permissions.Application.Commands.Permissions.DeletePermission
{
    public class DeletePermissionCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
