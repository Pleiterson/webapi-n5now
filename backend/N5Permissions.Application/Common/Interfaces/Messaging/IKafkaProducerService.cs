using System.Threading.Tasks;

namespace N5Permissions.Application.Common.Interfaces.Messaging;

public interface IKafkaProducerService
{
    Task PublishAsync(string topic, object @event);
}
