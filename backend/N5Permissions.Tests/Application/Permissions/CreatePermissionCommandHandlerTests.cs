using Moq;
using N5Permissions.Application.Commands.Permissions.CreatePermission;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Application.DTOs;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class CreatePermissionCommandHandlerTests
{
    private readonly Mock<IPermissionRepository> _permissionRepoMock;
    private readonly Mock<IKafkaProducerService> _producerMock;
    private readonly CreatePermissionCommandHandler _handler;

    public CreatePermissionCommandHandlerTests()
    {
        _permissionRepoMock = new Mock<IPermissionRepository>();
        _producerMock = new Mock<IKafkaProducerService>();

        _handler = new CreatePermissionCommandHandler(
            _permissionRepoMock.Object,
            _producerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Create_Permission_And_Publish_Event()
    {
        // Arrange
        var command = new CreatePermissionCommand
        {
            NombreEmpleado = "Pleiterson",
            ApellidoEmpleado = "Amorim",
            TipoPermiso = 1,
            FechaPermiso = DateTime.UtcNow
        };

        // Mock do repositório para retornar entidade com ID
        _permissionRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Permission>()))
            .ReturnsAsync((Permission p) =>
            {
                // Simula que o DB gerou o ID
                typeof(Permission)
                    .GetProperty("Id")
                    .SetValue(p, 10);

                return p;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Id);
        Assert.Equal("Pleiterson", result.NombreEmpleado);

        _permissionRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Permission>()),
            Times.Once);

        _producerMock.Verify(
            p => p.PublishAsync("permissions-created", It.IsAny<object>()),
            Times.Once);
    }
}
