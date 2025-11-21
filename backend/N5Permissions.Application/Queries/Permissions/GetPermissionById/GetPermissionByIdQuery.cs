using MediatR;
using N5Permissions.Application.DTOs;

namespace N5Permissions.Application.Queries.Permissions.GetPermissionById
{
    public class GetPermissionByIdQuery : IRequest<PermissionDto?>
    {
        public int Id { get; set; }
        public GetPermissionByIdQuery(int id) => Id = id;
    }
}
