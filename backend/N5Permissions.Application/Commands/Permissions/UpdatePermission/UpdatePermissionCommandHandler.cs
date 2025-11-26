using MediatR;
using N5Permissions.Domain.Repositories;
using N5Permissions.Application.Events.Permission;
using N5Permissions.Application.Common.Interfaces.Messaging;

namespace N5Permissions.Application.Commands.Permissions.UpdatePermission
{
    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, Unit>
    {
        private readonly IPermissionRepository _repository;
        private readonly IKafkaProducerService _producer;

        public UpdatePermissionCommandHandler(IPermissionRepository repository, IKafkaProducerService producer)
        {
            _repository = repository;
            _producer = producer;
        }

        public async Task<Unit> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException("Permission not found");

            entity.SetNombre(request.NombreEmpleado);
            entity.SetApellido(request.ApellidoEmpleado);
            entity.SetTipoPermiso(request.TipoPermiso);
            entity.SetFecha(request.FechaPermiso);

            await _repository.UpdateAsync(entity);

            var evt = new PermissionUpdatedEvent
            {
                Id = entity.Id,
                NombreEmpleado = entity.NombreEmpleado,
                ApellidoEmpleado = entity.ApellidoEmpleado,
                TipoPermiso = entity.TipoPermiso,
                FechaPermiso = entity.FechaPermiso
            };

            await _producer.PublishAsync("permissions-updated", evt);

            return Unit.Value;
        }
    }
}
