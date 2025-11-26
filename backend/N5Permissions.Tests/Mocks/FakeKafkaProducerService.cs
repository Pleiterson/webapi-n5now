using N5Permissions.Application.Common.Interfaces.Messaging;
using System.Threading.Tasks;

namespace N5Permissions.Tests.Integration.Fakes
{
    public class FakeKafkaProducerService : IKafkaProducerService
    {
        public Task PublishAsync(string topic, object @event)
        {
            // Não faz nada — só simula envio com sucesso
            return Task.CompletedTask;
        }
    }
}
