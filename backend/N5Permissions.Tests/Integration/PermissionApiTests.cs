using N5Permissions.Tests.Integration;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class PermissionApiTests : IClassFixture<ApiTestFixture>
{
    private readonly HttpClient _client;

    public PermissionApiTests(ApiTestFixture factory)
    {
        _client = factory.CreateClient();
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
        // Primeiro criar
        var created = await _client.PostAsJsonAsync("/api/permissions", new
        {
            nombreEmpleado = "Test",
            apellidoEmpleado = "User",
            tipoPermiso = 1,
            fechaPermiso = DateTime.UtcNow
        });

        var createdObj = await created.Content.ReadFromJsonAsync<dynamic>();
        int id = createdObj.id;

        // Agora atualizar
        var response = await _client.PutAsJsonAsync($"/api/permissions/{id}", new
        {
            id,
            nombreEmpleado = "Test UPDATED",
            apellidoEmpleado = "User UPDATED",
            tipoPermiso = 2,
            fechaPermiso = DateTime.UtcNow
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeletePermission_ShouldReturn204()
    {
        // criar
        var created = await _client.PostAsJsonAsync("/api/permissions", new
        {
            nombreEmpleado = "Delete",
            apellidoEmpleado = "Me",
            tipoPermiso = 1,
            fechaPermiso = DateTime.UtcNow
        });

        var createdObj = await created.Content.ReadFromJsonAsync<dynamic>();
        int id = createdObj.id;

        // deletar
        var res = await _client.DeleteAsync($"/api/permissions/{id}");

        Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);
    }
}
