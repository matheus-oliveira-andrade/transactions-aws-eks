using System;
using System.Globalization;
using System.Threading.Tasks;
using Movements.Application.Queries;
using Movements.Domain.Entities;
using Xunit;

namespace Movements.Api.Tests.QueryHandlers;

public class GetAccountMovementsReportQueryHandlerTests : IClassFixture<MovementsWebApplicationFixture>
{
    private readonly MovementsWebApplicationFixture _fixture;

    public GetAccountMovementsReportQueryHandlerTests(MovementsWebApplicationFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task ShouldGetMovementsReportOfAccount()
    {
        // Arrange
        const string accountId = "12345-67";

        var movement = await AddMovementAsync(accountId);

        var query = new GetAccountMovementsReportQuery(accountId);

        // Act
        var result = await _fixture.Mediator.Send(query);

        // Assert
        Assert.Equal(movement.Value, result.SpentByCategories["pix"]);
        Assert.Equal(movement.Value, result.TotalReceived);
        Assert.Equal(movement.Value, result.Balance);
        Assert.Equal(movement.Value, result.BalanceByMonths[DateTime.Today.ToString("MMMM", CultureInfo.InvariantCulture)]);
        Assert.Equal(0, result.TotalSpent);
    }

    private async Task<Movement> AddMovementAsync(string accountId)
    {
        var movement = new Movement(
            Guid.NewGuid(),
            accountId,
            DateTime.Today,
            105_50m,
            "pix",
            string.Empty);

        await _fixture.MovementRepository.AddAsync(movement);
        return movement;
    }
}