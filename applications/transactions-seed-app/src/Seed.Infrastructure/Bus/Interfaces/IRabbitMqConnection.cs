using RabbitMQ.Client;

namespace Seed.Infrastructure.Bus.Interfaces
{
    public interface IRabbitMqConnection
    {
        IConnection CreateConnection();
    }
}