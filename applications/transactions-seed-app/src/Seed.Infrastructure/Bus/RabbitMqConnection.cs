using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Seed.Infrastructure.Bus.Interfaces;
using Seed.Infrastructure.Bus.Options;

namespace Seed.Infrastructure.Bus
{
    [ExcludeFromCodeCoverage]
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly RabbitMqOptions _rabbitMqOptions;
        
        public RabbitMqConnection(IOptions<RabbitMqOptions> rabbitMqOptions)
        {
            _rabbitMqOptions = rabbitMqOptions.Value;
        }

        public IConnection CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password,
                HostName = _rabbitMqOptions.HostName,
                Port = 5672
            };

            return factory.CreateConnection();
        }
    }
}