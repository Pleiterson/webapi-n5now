namespace N5Permissions.Consumer.Settings;

public class KafkaConsumerSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string GroupId { get; set; } = "permissions-consumer-group";

    public TopicsSettings Topics { get; set; } = new();

    public class TopicsSettings
    {
        public string PermissionCreated { get; set; } = "permissions-created";
        public string PermissionUpdated { get; set; } = "permissions-updated";
        public string PermissionDeleted { get; set; } = "permissions-deleted";
        public string PermissionTypeCreated { get; set; } = "permissiontypes-created";
        public string PermissionTypeUpdated { get; set; } = "permissiontypes-updated";
        public string PermissionTypeDeleted { get; set; } = "permissiontypes-deleted";
    }
}
