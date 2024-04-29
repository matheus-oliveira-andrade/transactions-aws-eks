using System;
using Movements.Domain.Entities;
using Xunit;

namespace Movements.Domain.Tests.Entities;

public class MovementTests
{
    [Fact]
    public void ShouldCreate_WithSuccess()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = "1231-1";
        var today = DateTime.Today;
        var value = -899_99m;
        var category = "Food";
        var description = "XPTO";

        // Act
        var result = new Movement(
            transactionId,
            accountId,
            today, 
            value,
            category,
            description);

        // Assert
        Assert.NotEqual(default(Guid), result.Id);
        Assert.Equal(transactionId, result.TransactionId);
        Assert.Equal(accountId, result.AccountId);
        Assert.Equal(today, result.Date);
        Assert.Equal(value, result.Value);
        Assert.Equal(category, result.Category);
        Assert.Equal(description, result.Description);
    }
    
    [Fact]
    public void WhenTransactionIdIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var transactionId = Guid.Empty;
        var accountId = "1231-1";
        var today = DateTime.Today;
        var value = 899_99m;
        var category = "Food";
        var description = "XPTO";

        // Act & Assert 
        var exception = Assert.Throws<ArgumentException>(() => new Movement(
            transactionId,
            accountId,
            today,
            value,
            category,
            description));
        
        Assert.StartsWith("Can not be empty", exception.Message);
    }
    
    [Fact]
    public void WhenDateIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = "1231-1";
        var today = DateTime.MinValue;
        var value = 899_99m;
        var category = "Food";
        var description = "XPTO";

        // Act & Assert 
        var exception = Assert.Throws<ArgumentException>(() => new Movement(
            transactionId,
            accountId,
            today,
            value,
            category,
            description));
        
        Assert.StartsWith($"Can not be ${DateTime.MinValue} or ${DateTime.MaxValue}", exception.Message);
    }
    
    [Fact]
    public void WhenIsInvalidAccountId_ShouldThrowArgumentException()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        string accountId = "";
        var today = DateTime.MinValue;
        var value = 899_99m;
        var category = "Food";
        var description = "XPTO";

        // Act & Assert 
        Assert.Throws<ArgumentException>(() => new Movement(
            transactionId,
            accountId,
            today,
            value,
            category,
            description));
    }
    
    [Fact]
    public void WhenIsInvalidCategory_ShouldThrowArgumentException()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var accountId = "12311-1";
        var today = DateTime.MinValue;
        var value = 899_99m;
        var category = "";
        var description = "XPTO";

        // Act & Assert 
        Assert.Throws<ArgumentException>(() => new Movement(
            transactionId,
            accountId,
            today,
            value,
            category,
            description));
    }
}