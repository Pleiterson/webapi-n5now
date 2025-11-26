using MediatR;
using N5Permissions.Application.Common.Interfaces.Messaging;
using N5Permissions.Application.DTOs;
using N5Permissions.Application.Events.PermissionType;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace N5Permissions.Application.Commands.PermissionTypes.CreatePermissionType
{
    public class CreatePermissionTypeHandler : IRequestHandler<CreatePermissionTypeCommand, PermissionTypeDto>
    {
        private readonly IPermissionTypeRepository _repo;
        private readonly IKafkaProducerService _producer;

        public CreatePermissionTypeHandler(IPermissionTypeRepository repo, IKafkaProducerService producer)
        {
            _repo = repo;
            _producer = producer;
        }

        public async Task<PermissionTypeDto> Handle(CreatePermissionTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.PermissionType(request.Description);

            await _repo.AddAsync(entity);

            var evt = new PermissionTypeCreatedEvent
            {
                Id = entity.Id,
                Description = entity.Description
            };

            await _producer.PublishAsync("permissiontypes-created", evt);

            return new PermissionTypeDto
            {
                Id = entity.Id,
                Description = entity.Description
            };
        }
    }
}
