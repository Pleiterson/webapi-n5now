namespace N5Permissions.Infrastructure.Settings;

public class KafkaTopics
{
    public string PermissionCreated { get; set; } = string.Empty;
    public string PermissionUpdated { get; set; } = string.Empty;
    public string PermissionDeleted { get; set; } = string.Empty;

    public string PermissionTypeCreated { get; set; } = string.Empty;
    public string PermissionTypeUpdated { get; set; } = string.Empty;
    public string PermissionTypeDeleted { get; set; } = string.Empty;
}
