using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using N5Permissions.Consumer.Models;
using N5Permissions.Consumer.Settings;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace N5Permissions.Consumer.Services;

public class ElasticSearchService
{
    private readonly ElasticLowLevelClient _client;
    private readonly ElasticSettings _settings;
    private readonly ILogger<ElasticSearchService> _logger;

    public ElasticSearchService(
        IOptions<ElasticSettings> settings,
        ILogger<ElasticSearchService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        var pool = new SingleNodeConnectionPool(new Uri(_settings.Url));
        var config = new ConnectionConfiguration(pool);

        _client = new ElasticLowLevelClient(config);
    }

    public async Task IndexPermissionAsync(PermissionDocument doc)
    {
        var response = await _client.IndexAsync<StringResponse>(
            _settings.IndexPermissions,
            doc.Id.ToString(),
            PostData.Serializable(doc));

        _logger.LogInformation("Indexed Permission ID {Id} - Status {Status}",
            doc.Id, response.HttpStatusCode);
    }

    public async Task IndexPermissionTypeAsync(PermissionTypeDocument doc)
    {
        var response = await _client.IndexAsync<StringResponse>(
            _settings.IndexPermissionTypes,
            doc.Id.ToString(),
            PostData.Serializable(doc));

        _logger.LogInformation("Indexed PermissionType ID {Id} - Status {Status}",
            doc.Id, response.HttpStatusCode);
    }

    public async Task DeletePermissionAsync(int id)
    {
        await _client.DeleteAsync<StringResponse>(
            _settings.IndexPermissions, id.ToString());
    }

    public async Task DeletePermissionTypeAsync(int id)
    {
        await _client.DeleteAsync<StringResponse>(
            _settings.IndexPermissionTypes, id.ToString());
    }
}
