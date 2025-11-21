using Moq;
using N5Permissions.Application.Commands.Permissions.UpdatePermission;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class UpdatePermissionCommandHandlerTests
{
    private readonly Mock<IPermissionRepository> _repoMock;
    private readonly Mock<IKafkaProducerService> _producerMock;
    private readonly UpdatePermissionCommandHandler _handler;

    public UpdatePermissionCommandHandlerTests()
    {
        _repoMock = new Mock<IPermissionRepository>();
        _producerMock = new Mock<IKafkaProducerService>();

        _handler = new UpdatePermissionCommandHandler(
            _repoMock.Object,
            _producerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Update_Existing_Permission()
    {
        // Arrange
        var dbEntity = new Permission(
            "John",
            "Doe",
            1,
            DateTime.UtcNow
        );

        typeof(Permission)
            .GetProperty("Id")!
            .SetValue(dbEntity, 5);

        _repoMock.Setup(r => r.GetByIdAsync(5))
                 .ReturnsAsync(dbEntity);

        var command = new UpdatePermissionCommand
        {
            Id = 5,
            NombreEmpleado = "Pleiterson",
            ApellidoEmpleado = "Amorim",
            TipoPermiso = 2,
            FechaPermiso = DateTime.UtcNow
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Pleiterson", dbEntity.NombreEmpleado);
        Assert.Equal(2, dbEntity.TipoPermiso);

        _repoMock.Verify(r => r.UpdateAsync(dbEntity), Times.Once);
        _producerMock.Verify(p =>
            p.PublishAsync("permissions-updated", It.IsAny<object>()),
            Times.Once);
    }
}
