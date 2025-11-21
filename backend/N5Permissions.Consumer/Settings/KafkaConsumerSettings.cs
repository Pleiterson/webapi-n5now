namespace N5Permissions.Consumer.Settings;

public class KafkaConsumerSettings
{
    public string BootstrapServers { get; set; } = "";
    public string GroupId { get; set; } = "";

    public TopicsSettings Topics { get; set; } = new();

    public class TopicsSettings
    {
        public string PermissionCreated { get; set; } = "";
        public string PermissionUpdated { get; set; } = "";
        public string PermissionDeleted { get; set; } = "";
        public string PermissionTypeCreated { get; set; } = "";
        public string PermissionTypeUpdated { get; set; } = "";
        public string PermissionTypeDeleted { get; set; } = "";
    }
}
