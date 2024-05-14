using RabbitMQ.Client;

namespace Movements.Domain.Interfaces.Bus;

public interface IBusService
{
    void BindConsumerInQueue(string queue, string exchange, string routingKey, IBasicConsumer consumer);
}