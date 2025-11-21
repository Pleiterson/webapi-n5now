using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Permissions.Consumer.Settings;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Consumer.Services;

public class KafkaConsumerService : BackgroundService
{
    private readonly ILogger<KafkaConsumerService> _logger;
    private readonly KafkaConsumerSettings _settings;
    private readonly EventProcessorService _processor;

    public KafkaConsumerService(
        IOptions<KafkaConsumerSettings> settings,
        EventProcessorService processor,
        ILogger<KafkaConsumerService> logger)
    {
        _processor = processor;
        _logger = logger;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            GroupId = _settings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<string, string>(config).Build();

        consumer.Subscribe(new[]
        {
            _settings.Topics.PermissionCreated,
            _settings.Topics.PermissionUpdated,
            _settings.Topics.PermissionDeleted,
            _settings.Topics.PermissionTypeCreated,
            _settings.Topics.PermissionTypeUpdated,
            _settings.Topics.PermissionTypeDeleted
        });

        _logger.LogInformation("Kafka consumer iniciado...");

        while (!stoppingToken.IsCancellationRequested)
        {
            var result = consumer.Consume(stoppingToken);

            _logger.LogInformation("Received message from topic {Topic}", result.Topic);

            await _processor.ProcessAsync(result.Topic, result.Message.Value);
        }
    }
}
