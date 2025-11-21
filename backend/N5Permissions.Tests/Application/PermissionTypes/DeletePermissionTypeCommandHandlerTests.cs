using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using N5Permissions.Application.Commands.PermissionTypes.DeletePermissionType;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using N5Permissions.Application.Common.Interfaces.Messaging;

public class DeletePermissionTypeCommandHandlerTests
{
    private readonly Mock<IPermissionTypeRepository> _repoMock;
    private readonly Mock<IKafkaProducerService> _producerMock;
    private readonly DeletePermissionTypeHandler _handler;

    public DeletePermissionTypeCommandHandlerTests()
    {
        _repoMock = new Mock<IPermissionTypeRepository>();
        _producerMock = new Mock<IKafkaProducerService>();

        _handler = new DeletePermissionTypeHandler(
            _repoMock.Object,
            _producerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Delete_PermissionType()
    {
        // Arrange
        var existing = new PermissionType("Temp");
        typeof(PermissionType).GetProperty("Id")!.SetValue(existing, 77);

        _repoMock.Setup(r => r.GetByIdAsync(77))
                 .ReturnsAsync(existing);

        var command = new DeletePermissionTypeCommand { Id = 77 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);

        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
        _producerMock.Verify(p =>
            p.PublishAsync("permissiontypes-deleted", It.IsAny<object>()),
            Times.Once);
    }
}
