namespace N5Permissions.Infrastructure.Settings;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public KafkaTopics Topics { get; set; } = new KafkaTopics();
    public bool Enable { get; set; } = false;
}
