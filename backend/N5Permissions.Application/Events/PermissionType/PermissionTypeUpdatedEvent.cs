namespace N5Permissions.Application.Events.PermissionType
{
    public class PermissionTypeUpdatedEvent
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
