using Moq;
using N5Permissions.Application.Commands.Permissions.DeletePermission;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class DeletePermissionCommandHandlerTests
{
    private readonly Mock<IPermissionRepository> _repoMock;
    private readonly Mock<IKafkaProducerService> _producerMock;
    private readonly DeletePermissionCommandHandler _handler;

    public DeletePermissionCommandHandlerTests()
    {
        _repoMock = new Mock<IPermissionRepository>();
        _producerMock = new Mock<IKafkaProducerService>();

        _handler = new DeletePermissionCommandHandler(
            _repoMock.Object,
            _producerMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_Delete_Existing_Permission()
    {
        // Arrange
        var dbEntity = new Permission("A", "B", 1, DateTime.UtcNow);
        typeof(Permission).GetProperty("Id")!.SetValue(dbEntity, 10);

        _repoMock.Setup(r => r.GetByIdAsync(10))
                 .ReturnsAsync(dbEntity);

        var command = new DeletePermissionCommand { Id = 10 };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(dbEntity), Times.Once);
        _producerMock.Verify(p =>
            p.PublishAsync("permissions-deleted", It.IsAny<object>()),
            Times.Once);
    }
}
