using Moq;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using N5Permissions.Application.Commands.PermissionTypes.UpdatePermissionType;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using N5Permissions.Application.Common.Interfaces.Messaging;

public class UpdatePermissionTypeCommandHandlerTests
{
	private readonly Mock<IPermissionTypeRepository> _repoMock;
	private readonly Mock<IKafkaProducerService> _producerMock;
	private readonly UpdatePermissionTypeHandler _handler;

	public UpdatePermissionTypeCommandHandlerTests()
	{
		_repoMock = new Mock<IPermissionTypeRepository>();
		_producerMock = new Mock<IKafkaProducerService>();

		_handler = new UpdatePermissionTypeHandler(
			_repoMock.Object,
			_producerMock.Object
		);
	}

	[Fact]
	public async Task Handle_Should_Update_PermissionType()
	{
		// Arrange
		var existing = new PermissionType("Old Description");
		typeof(PermissionType).GetProperty("Id")!.SetValue(existing, 50);

		_repoMock.Setup(r => r.GetByIdAsync(50)).ReturnsAsync(existing);

		var command = new UpdatePermissionTypeCommand
		{
			Id = 50,
			Description = "Updated Description"
		};

		// Act
		var result = await _handler.Handle(command, CancellationToken.None);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(50, result.Id);
		Assert.Equal("Updated Description", existing.Description);

		_repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
		_producerMock.Verify(p =>
			p.PublishAsync("permissiontypes-updated", It.IsAny<object>()),
			Times.Once);
	}
}
