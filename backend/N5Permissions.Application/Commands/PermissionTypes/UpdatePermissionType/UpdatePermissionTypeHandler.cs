using MediatR;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Application.DTOs;
using N5Permissions.Application.Events.PermissionType;
using N5Permissions.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Commands.PermissionTypes.UpdatePermissionType
{
    public class UpdatePermissionTypeHandler : IRequestHandler<UpdatePermissionTypeCommand, PermissionTypeDto?>
    {
        private readonly IPermissionTypeRepository _repo;
        private readonly IKafkaProducerService _producer;

        public UpdatePermissionTypeHandler(IPermissionTypeRepository repo, IKafkaProducerService producer)
        {
            _repo = repo;
            _producer = producer;
        }

        public async Task<PermissionTypeDto?> Handle(UpdatePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null)
                return null;

            entity.SetDescription(request.Description);
            await _repo.UpdateAsync(entity);

            var evt = new PermissionTypeUpdatedEvent
            {
                Id = entity.Id,
                Description = entity.Description
            };

            await _producer.PublishAsync("permission-types", evt);

            return new PermissionTypeDto
            {
                Id = entity.Id,
                Description = entity.Description
            };
        }
    }
}
