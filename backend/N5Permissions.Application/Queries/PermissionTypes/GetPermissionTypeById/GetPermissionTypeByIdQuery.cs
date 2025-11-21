using MediatR;
using N5Permissions.Application.DTOs;

namespace N5Permissions.Application.Queries.PermissionTypes.GetPermissionTypeById
{
    public class GetPermissionTypeByIdQuery : IRequest<PermissionTypeDto?>
    {
        public int Id { get; set; }
        public GetPermissionTypeByIdQuery(int id) => Id = id;
    }
}
