using System;
using System.Threading.Tasks;
using FluentAssertions;
using Movements.Application.Events;
using Xunit;

namespace Movements.Api.Tests.EventHandlers;

public class TransactionProcessedEventHandlerTests : IClassFixture<MovementsWebApplicationFixture>
{
    private readonly MovementsWebApplicationFixture _fixture;

    public TransactionProcessedEventHandlerTests(MovementsWebApplicationFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task ShouldProcessAndCreateMovementForAccount()
    {
        // Arrange
        var @event = new TransactionProcessed
        {
            TransactionId = Guid.NewGuid(),
            Category = "pix",
            AccountId = "12345-67",
            Value = -50_60m,
            Description = string.Empty,
            ProcessingDate = DateTime.UtcNow.AddDays(-1)
        };
        
        // Act
        await _fixture.Mediator.Send(@event);
        var movements = await _fixture.MovementRepository.GetByAccountIdAsync(@event.AccountId);

        // Assert
        movements.Should().NotBeEmpty();
        movements.Should().HaveCount(1);
        movements.Should().ContainSingle(m => m.TransactionId == @event.TransactionId);
    }
}