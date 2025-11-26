using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using N5Permissions.Consumer.Services;
using N5Permissions.Consumer.Settings;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.Configure<KafkaConsumerSettings>(
            context.Configuration.GetSection("Kafka"));

        services.Configure<ElasticSettings>(
            context.Configuration.GetSection("ElasticSearch"));

        services.AddSingleton<IElasticSearchService, ElasticSearchService>();
        services.AddHostedService<KafkaConsumerService>();
        services.AddSingleton<EventProcessorService>();
    })
    .Build()
    .Run();
