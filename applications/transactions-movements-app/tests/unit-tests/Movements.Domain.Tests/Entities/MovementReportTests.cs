using System;
using System.Collections.Generic;
using System.Linq;
using Movements.Domain.Entities;
using Xunit;

namespace Movements.Domain.Tests.Entities;

public class MovementReportTests
{
    [Fact]
    public void WhenIsEmpty_ShouldThrowArgumentException()
    {
        // Arrange
        var movements = Enumerable.Empty<Movement>().ToList();
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new MovementReport(movements));
    }
    
    [Fact]
    public void WhenHasItemsNull_ShouldThrowArgumentException()
    {
        // Arrange
        var movements = new List<Movement>
        {
            new(Guid.NewGuid(), "11111-1", new DateTime(2023, 03, 28), -100, "FOOD", string.Empty),
            null
        };
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new MovementReport(movements));
    }
    
    [Fact]
    public void ShouldCalculateCorrectly_WhenCreated()
    {
        // Arrange
        var movements = new List<Movement>
        {
            new(Guid.NewGuid(), "11111-1", new DateTime(2023, 03, 28), -100, "FOOD", string.Empty),
            new(Guid.NewGuid(), "11111-1", new DateTime(2023, 03, 28), -200, "FOOD", string.Empty),
            new(Guid.NewGuid(), "11111-1", new DateTime(2023, 03, 28), 150, "PIX", string.Empty),
            new(Guid.NewGuid(), "11111-1", new DateTime(2023, 02, 28), 50, "PIX", string.Empty),
        };
        
        // Act
        var result = new MovementReport(movements);
        
        // Assert
        Assert.Equal(-100, result.Balance);
        Assert.Equal(200, result.TotalReceived);
        Assert.Equal(-300, result.TotalSpent);
        
        Assert.Equal(2, result.SpentByCategories.Count);
        Assert.Equal(-300, result.SpentByCategories["FOOD"]);
        Assert.Equal(200, result.SpentByCategories["PIX"]);
        
        Assert.Equal(2, result.BalanceByMonths.Count);
        Assert.Equal(-150, result.BalanceByMonths["March"]);
        Assert.Equal(50, result.BalanceByMonths["February"]);
    }
}