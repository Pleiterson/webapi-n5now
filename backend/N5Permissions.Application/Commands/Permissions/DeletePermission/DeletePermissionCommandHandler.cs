using MediatR;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Application.Events.Permission;
using N5Permissions.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Commands.Permissions.DeletePermission
{
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, Unit>
    {
        private readonly IPermissionRepository _repository;
        private readonly IKafkaProducerService _producer;

        public DeletePermissionCommandHandler(IPermissionRepository repo, IKafkaProducerService producer)
        {
            _repository = repo;
            _producer = producer;
        }

        public async Task<Unit> Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
                return Unit.Value;

            await _repository.DeleteAsync(entity);

            var evt = new PermissionDeletedEvent
            {
                Id = request.Id
            };

            await _producer.PublishAsync("permissions-deleted", evt);

            return Unit.Value;
        }
    }
}
