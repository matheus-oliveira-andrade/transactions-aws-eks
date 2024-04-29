using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Movements.Application.EventHandlers;
using Movements.Application.Events;
using Movements.Domain.Entities;
using Movements.Domain.Interfaces.Data;
using Xunit;

namespace Movements.Application.Tests.EventHandlers;

public class TransactionProcessedEventHandlerTests
{
    private readonly TransactionProcessedEventHandler _handler;
    private readonly Mock<IMovementRepository> _movementRepositoryMock;

    public TransactionProcessedEventHandlerTests()
    {
        _movementRepositoryMock = new Mock<IMovementRepository>();
        var loggerMock = new Mock<ILogger<TransactionProcessedEventHandler>>();

        _handler = new TransactionProcessedEventHandler(loggerMock.Object, _movementRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateMovement_WithSuccess()
    {
        // Arrange
        var @event = new TransactionProcessed
        {
            TransactionId = Guid.NewGuid(),
            AccountId = "1231-11",
            ProcessingDate = DateTime.Today,
            Value = -1,
            Category = "category",
            Description = ""
        };
        
        // Act
        await _handler.Handle(@event, CancellationToken.None);

        // Assert 
        _movementRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Movement>(), CancellationToken.None));
    }
    
    [Fact]
    public async Task Handle_ShouldThrowsException_WhenIsInvalidTransaction()
    {
        // Arrange
        var @event = new TransactionProcessed
        {
            TransactionId = Guid.NewGuid(),
            AccountId = "1231-11",
            ProcessingDate = DateTime.Today,
            Value = -1,
            Category = null,
            Description = ""
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(@event, CancellationToken.None));
    }
}