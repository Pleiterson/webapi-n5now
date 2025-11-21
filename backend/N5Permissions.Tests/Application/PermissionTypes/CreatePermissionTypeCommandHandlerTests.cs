using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using N5Permissions.Application.Commands.PermissionTypes.CreatePermissionType;
using N5Permissions.Application.DTOs;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using N5Permissions.Application.Common.Interfaces.Messaging;

public class CreatePermissionTypeCommandHandlerTests
{
    private readonly Mock<IPermissionTypeRepository> _repoMock;
    private readonly Mock<IKafkaProducerService> _producerMock;
    private readonly CreatePermissionTypeHandler _handler;

    public CreatePermissionTypeCommandHandlerTests()
    {
        _repoMock = new Mock<IPermissionTypeRepository>();
        _producerMock = new Mock<IKafkaProducerService>();

        _handler = new CreatePermissionTypeHandler(
            _repoMock.Object,
            _producerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Create_PermissionType_And_Publish_Event()
    {
        // Arrange
        var command = new CreatePermissionTypeCommand
        {
            Description = "Vacaciones"
        };

        _repoMock.Setup(r => r.AddAsync(It.IsAny<PermissionType>()))
            .ReturnsAsync((PermissionType p) =>
            {
                typeof(PermissionType)
                    .GetProperty("Id")!
                    .SetValue(p, 100);

                return p;
            });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.Id);
        Assert.Equal("Vacaciones", result.Description);

        _repoMock.Verify(r => r.AddAsync(It.IsAny<PermissionType>()), Times.Once);
        _producerMock.Verify(p =>
            p.PublishAsync("permissiontypes-created", It.IsAny<object>()),
            Times.Once);
    }
}
