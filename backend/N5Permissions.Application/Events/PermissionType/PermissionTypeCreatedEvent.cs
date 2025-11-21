namespace N5Permissions.Application.Events.PermissionType
{
    public class PermissionTypeCreatedEvent
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
