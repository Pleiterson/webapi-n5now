namespace N5Permissions.Application.DTOs
{
    public class UpdatePermissionTypeRequest
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
