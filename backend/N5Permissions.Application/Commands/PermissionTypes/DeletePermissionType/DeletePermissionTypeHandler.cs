using MediatR;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Application.Events.PermissionType;
using N5Permissions.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Commands.PermissionTypes.DeletePermissionType
{
    public class DeletePermissionTypeHandler : IRequestHandler<DeletePermissionTypeCommand, bool>
    {
        private readonly IPermissionTypeRepository _repo;
        private readonly IKafkaProducerService _producer;

        public DeletePermissionTypeHandler(IPermissionTypeRepository repo, IKafkaProducerService producer)
        {
            _repo = repo;
            _producer = producer;
        }

        public async Task<bool> Handle(DeletePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null)
                return false;

            await _repo.DeleteAsync(entity);

            var evt = new PermissionTypeDeletedEvent
            {
                Id = request.Id
            };

            await _producer.PublishAsync("permission-types", evt);

            return true;
        }
    }
}
