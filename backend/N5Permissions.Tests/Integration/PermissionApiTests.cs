using N5Permissions.Tests.Integration;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

public class PermissionApiTests : IClassFixture<ApiTestFixture>
{
    private readonly HttpClient _client;

    public PermissionApiTests(ApiTestFixture factory)
    {
        _client = factory.Client;
    }

    [Fact]
    public async Task CreatePermission_ShouldReturn201()
    {
        var payload = new
        {
            nombreEmpleado = "Pleiterson",
            apellidoEmpleado = "Amorim",
            tipoPermiso = 1,
            fechaPermiso = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/permissions", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetPermissions_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/permissions");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdatePermission_ShouldReturn200()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/permissions", new
        {
            nombreEmpleado = "John",
            apellidoEmpleado = "Doe",
            tipoPermiso = 1,
            fechaPermiso = DateTime.UtcNow
        });

        var createdJson = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var id = createdJson.GetProperty("id").GetInt32();

        var updateResponse = await _client.PutAsJsonAsync($"/api/permissions/{id}", new
        {
            nombreEmpleado = "Updated",
            apellidoEmpleado = "Updated",
            tipoPermiso = 1,
            fechaPermiso = DateTime.UtcNow
        });

        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
    }

    [Fact]
    public async Task DeletePermission_ShouldReturn204()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/permissions", new
        {
            nombreEmpleado = "John",
            apellidoEmpleado = "Doe",
            tipoPermiso = 1,
            fechaPermiso = DateTime.UtcNow
        });

        var json = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var id = json.GetProperty("id").GetInt32();

        var deleteResponse = await _client.DeleteAsync($"/api/permissions/{id}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}
