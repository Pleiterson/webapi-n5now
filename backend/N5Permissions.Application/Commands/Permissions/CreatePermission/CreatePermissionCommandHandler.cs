using MediatR;
using N5Permissions.Domain.Entities;
using N5Permissions.Domain.Repositories;
using N5Permissions.Application.Events.Permission;
using N5Permissions.Application.Common.Interfaces.Messaging;

namespace N5Permissions.Application.Commands.Permissions.CreatePermission
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, int>
    {
        private readonly IPermissionRepository _repository;
        private readonly IKafkaProducerService _producer;

        public CreatePermissionCommandHandler(
            IPermissionRepository repository,
            IKafkaProducerService producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task<int> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = new Permission(
                request.NombreEmpleado,
                request.ApellidoEmpleado,
                request.TipoPermiso,
                request.FechaPermiso
            );

            await _repository.AddAsync(entity);

            var evt = new PermissionCreatedEvent
            {
                Id = entity.Id,
                NombreEmpleado = entity.NombreEmpleado,
                ApellidoEmpleado = entity.ApellidoEmpleado,
                TipoPermiso = entity.TipoPermiso,
                FechaPermiso = entity.FechaPermiso
            };

            await _producer.PublishAsync("permissions-created", evt);

            return entity.Id;
        }
    }
}
