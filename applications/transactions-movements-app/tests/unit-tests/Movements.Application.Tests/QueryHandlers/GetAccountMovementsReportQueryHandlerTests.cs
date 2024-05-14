using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Movements.Application.Queries;
using Movements.Application.QueryHandlers;
using Movements.Domain.Entities;
using Movements.Domain.Interfaces.Data;
using Xunit;

namespace Movements.Application.Tests.QueryHandlers;

public class GetAccountMovementsReportQueryHandlerTests
{
    private readonly GetAccountMovementsReportQueryHandler _handler;
    private readonly Mock<IMovementRepository> _movementRepositoryMock;

    public GetAccountMovementsReportQueryHandlerTests()
    {
        _movementRepositoryMock = new Mock<IMovementRepository>();
        var loggerMock = new Mock<ILogger<GetAccountMovementsReportQueryHandler>>();

        _handler = new GetAccountMovementsReportQueryHandler(loggerMock.Object, _movementRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenNotFoundMovements()
    {
        // Arrange
        var query = new GetAccountMovementsReportQuery("11111-1");
        
        _movementRepositoryMock
            .Setup(x => x.GetByAccountIdAsync(query.AccountId))
            .ReturnsAsync(new List<Movement>());
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert 
        Assert.Null(result);
        
        _movementRepositoryMock.Verify(x => x.GetByAccountIdAsync(query.AccountId));
    }
    
    [Fact]
    public async Task Handle_ShouldReturnMovementsReport_WhenNotFoundMovements()
    {
        // Arrange
        var query = new GetAccountMovementsReportQuery("11111-1");
        
        _movementRepositoryMock
            .Setup(x => x.GetByAccountIdAsync(query.AccountId))
            .ReturnsAsync(new List<Movement>
            {
                new(Guid.NewGuid(), "1231-1", DateTime.Today, 56_00, "XPTO","")
            });
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert 
        Assert.NotNull(result);
        
        _movementRepositoryMock.Verify(x => x.GetByAccountIdAsync(query.AccountId));
    }
}