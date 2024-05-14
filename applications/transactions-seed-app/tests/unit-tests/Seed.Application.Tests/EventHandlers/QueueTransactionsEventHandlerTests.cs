using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Seed.Application.EventHandlers;
using Seed.Application.Events;
using Seed.Domain.Entities;
using Seed.Domain.Interfaces.Bus;
using Seed.Domain.Interfaces.Repositories;
using Xunit;

namespace Seed.Application.Tests.EventHandlers
{
    public class QueueTransactionsEventHandlerTests
    {
        private readonly QueueTransactionsEventHandler _handler;
        private readonly Mock<IBusService> _busServiceMock;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;

        public QueueTransactionsEventHandlerTests()
        {
            var loggerMock = new Mock<ILogger<QueueTransactionsEventHandler>>();
            _busServiceMock = new Mock<IBusService>();
            _transactionRepositoryMock = new Mock<ITransactionRepository>();

            _handler = new QueueTransactionsEventHandler(
                loggerMock.Object,
                _busServiceMock.Object,
                _transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldPublishTransactionsInBus()
        {
            // Arrange
            _transactionRepositoryMock
                .Setup(x => x.GetByRandomAccountIdAsync())
                .ReturnsAsync(new List<Transaction>
                {
                    new(Guid.NewGuid(), "1", 1500_00m, DateTime.UtcNow, string.Empty, string.Empty),
                    new(Guid.NewGuid(), "1", 88_99m, DateTime.UtcNow, string.Empty, string.Empty),
                    new(Guid.NewGuid(), "1", 110_00m, DateTime.UtcNow, string.Empty, string.Empty),
                });

            // Act
            await _handler.Handle(new QueueTransactionsEvent(), CancellationToken.None);

            // Assert
            _busServiceMock.Verify(x => 
                x.Publish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<List<string>>()));
        }
    }
}