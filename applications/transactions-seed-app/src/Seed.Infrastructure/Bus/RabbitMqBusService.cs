using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using Seed.Domain.Interfaces.Bus;
using Seed.Infrastructure.Bus.Interfaces;

namespace Seed.Infrastructure.Bus
{
    public class RabbitMqBusService : IBusService
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;

        public RabbitMqBusService(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void Publish(string exchangeName, string key, List<string> messages)
        {
            using var connection = _rabbitMqConnection.CreateConnection();
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);
            
            foreach (var message in messages)
            {
                var properties = channel.CreateBasicProperties();
                properties.ContentType = "application/json";
                properties.MessageId = Guid.NewGuid().ToString();

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchangeName, key, properties, body);
            }
        }
    }
}