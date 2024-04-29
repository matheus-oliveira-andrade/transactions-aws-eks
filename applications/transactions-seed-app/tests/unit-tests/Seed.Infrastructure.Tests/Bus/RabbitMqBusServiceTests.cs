using System.Collections.Generic;
using Moq;
using RabbitMQ.Client;
using Seed.Domain.Interfaces.Bus;
using Seed.Infrastructure.Bus;
using Seed.Infrastructure.Bus.Interfaces;
using Xunit;

namespace Seed.Infrastructure.Tests.Bus
{
    public class RabbitMqBusServiceTests
    {
        private readonly Mock<IRabbitMqConnection> _rabbitMqConnectionMock;
        
        private readonly IBusService _busService;

        public RabbitMqBusServiceTests()
        {
            _rabbitMqConnectionMock = new Mock<IRabbitMqConnection>();
            
            _busService = new RabbitMqBusService(_rabbitMqConnectionMock.Object);
        }


        [Fact]
        public void Public_ShouldDeclareExchangeAndPublishMessages_WithSuccess()
        {
            // Arrange
            var connectionMock = new Mock<IConnection>(); 
            var modelMock = new Mock<IModel>();

            connectionMock
                .Setup(x => x.CreateModel())
                .Returns(modelMock.Object);
            modelMock
                .Setup(x => x.CreateBasicProperties())
                .Returns(Mock.Of<IBasicProperties>());
            
            _rabbitMqConnectionMock
                .Setup(x => x.CreateConnection())
                .Returns(connectionMock.Object);

            // Act
            _busService.Publish("exchange", "key", new List<string> { "message1", "message2"});

            // Assert
            modelMock.Verify(x => 
                x.ExchangeDeclare("exchange", ExchangeType.Fanout, true, false, null));
        }
    }
}