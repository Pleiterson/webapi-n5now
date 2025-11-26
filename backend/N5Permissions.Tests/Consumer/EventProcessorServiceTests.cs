using Moq;
using N5Permissions.Consumer.Models;
using N5Permissions.Consumer.Services;
using N5Permissions.Application.Events.Permission;
using N5Permissions.Application.Events.PermissionType;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

public class EventProcessorServiceTests
{
    private readonly Mock<IElasticSearchService> _elasticMock;
    private readonly EventProcessorService _processor;

    public EventProcessorServiceTests()
    {
        _elasticMock = new Mock<IElasticSearchService>();

        _processor = new EventProcessorService(_elasticMock.Object);
    }

    // Permission Created
    [Fact]
    public async Task ProcessAsync_Should_Handle_PermissionCreated()
    {
        var mock = new Mock<IElasticSearchService>();
        var processor = new EventProcessorService(mock.Object);

        var message = "{\"Id\":1,\"NombreEmpleado\":\"Test\",\"ApellidoEmpleado\":\"User\",\"TipoPermiso\":1,\"FechaPermiso\":\"2024-01-01\"}";

        await processor.ProcessAsync("permissions-created", message);

        mock.Verify(e => e.IndexPermissionAsync(It.IsAny<PermissionDocument>()), Times.Once);
    }

    // Permission Updated
    [Fact]
    public async Task ProcessAsync_Should_Handle_PermissionUpdated()
    {
        var evt = new PermissionUpdatedEvent
        {
            Id = 5,
            NombreEmpleado = "Updated",
            ApellidoEmpleado = "Test",
            TipoPermiso = 2,
            FechaPermiso = DateTime.UtcNow
        };

        var json = JsonSerializer.Serialize(evt);

        await _processor.ProcessAsync("permissions-updated", json);

        _elasticMock.Verify(e =>
            e.IndexPermissionAsync(It.Is<PermissionDocument>(d =>
                d.Id == evt.Id &&
                d.NombreEmpleado == evt.NombreEmpleado &&
                d.ApellidoEmpleado == evt.ApellidoEmpleado &&
                d.TipoPermiso == evt.TipoPermiso)),
            Times.Once);
    }

    // Permission Deleted
    [Fact]
    public async Task ProcessAsync_Should_Handle_PermissionDeleted()
    {
        var evt = new PermissionDeletedEvent { Id = 7 };
        var json = JsonSerializer.Serialize(evt);

        await _processor.ProcessAsync("permissions-deleted", json);

        _elasticMock.Verify(e =>
            e.DeletePermissionAsync(evt.Id),
            Times.Once);
    }

    // PermissionType Created
    [Fact]
    public async Task ProcessAsync_Should_Handle_PermissionTypeCreated()
    {
        var evt = new PermissionTypeCreatedEvent
        {
            Id = 3,
            Description = "Test"
        };

        var json = JsonSerializer.Serialize(evt);

        await _processor.ProcessAsync("permissiontypes-created", json);

        _elasticMock.Verify(e =>
            e.IndexPermissionTypeAsync(It.Is<PermissionTypeDocument>(d =>
                d.Id == evt.Id &&
                d.Description == evt.Description)),
            Times.Once);
    }

    // PermissionType Updated
    [Fact]
    public async Task ProcessAsync_Should_Handle_PermissionTypeUpdated()
    {
        var evt = new PermissionTypeUpdatedEvent
        {
            Id = 3,
            Description = "Updated Type"
        };

        var json = JsonSerializer.Serialize(evt);

        await _processor.ProcessAsync("permissiontypes-updated", json);

        _elasticMock.Verify(e =>
            e.IndexPermissionTypeAsync(It.Is<PermissionTypeDocument>(d =>
                d.Id == evt.Id &&
                d.Description == evt.Description)),
            Times.Once);
    }

    // PermissionType Deleted
    [Fact]
    public async Task ProcessAsync_Should_Handle_PermissionTypeDeleted()
    {
        var evt = new PermissionTypeDeletedEvent { Id = 3 };
        var json = JsonSerializer.Serialize(evt);

        await _processor.ProcessAsync("permissiontypes-deleted", json);

        _elasticMock.Verify(e =>
            e.DeletePermissionTypeAsync(evt.Id),
            Times.Once);
    }
}
