using Moq;
using N5Permissions.Application.Commands.Permissions.CreatePermission;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using System;
using System.Reflection;
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
    public async Task Handle_Should_Return_New_Id_And_Publish_Event()
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
                // cria a mesma entidade (ou use a passada) e define Id via reflection
                var prop = typeof(Permission).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                {
                    // usa reflection para setar o id (setter é privado no entity)
                    prop.SetValue(p, 10);
                }
                return p;
            });

        // Act
        var resultId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(10, resultId);

        _permissionRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Permission>()),
            Times.Once);

        _producerMock.Verify(
            p => p.PublishAsync("permissions-created", It.IsAny<object>()),
            Times.Once);
    }
}
