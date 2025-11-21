using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Infrastructure.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace N5Permissions.Infrastructure.Messaging;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly KafkaSettings _settings;

    public KafkaProducerService(
        IOptions<KafkaSettings> settings,
        ILogger<KafkaProducerService> logger)
    {
        _logger = logger;
        _settings = settings.Value;

        var config = new ProducerConfig
        {
            BootstrapServers = _settings.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            MessageTimeoutMs = 5000,
            RetryBackoffMs = 1000
        };

        _producer = new ProducerBuilder<string, string>(config)
            .SetErrorHandler((_, e) =>
                _logger.LogError("Kafka error: {Error}", e.Reason))
            .Build();
    }

    public async Task PublishAsync(string topic, object @event)
    {
        try
        {
            var message = new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonSerializer.Serialize(@event)
            };

            var result = await _producer.ProduceAsync(topic, message);

            _logger.LogInformation(
                "Mensagem publicada no tópico {Topic}. Offset: {Offset}",
                topic, result.Offset
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar evento no Kafka");
            throw;
        }
    }
}
