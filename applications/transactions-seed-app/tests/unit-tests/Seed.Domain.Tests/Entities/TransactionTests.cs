using System;
using Seed.Domain.Entities;
using Xunit;

namespace Seed.Domain.Tests.Entities
{
    public class TransactionTests
    {
        [Fact]
        public void Transaction_ConstructedWithValidParameter_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            const string accountId = "12341-1";
            const decimal value = 123_1m;
            var processingDate = DateTime.UtcNow.AddHours(-3);
            const string description = "* IFood";
            const string category = "Food";

            // Act
            var result = new Transaction(transactionId, accountId, value, processingDate, description, category);

            // Assert
            Assert.Equal(transactionId, result.TransactionId);
            Assert.Equal(accountId, result.AccountId);
            Assert.Equal(value, result.Value);
            Assert.Equal(processingDate, result.ProcessingDate);
            Assert.Equal(description, result.Description);
            Assert.Equal(category, result.Category);
        }
        
        [Fact]
        public void Transaction_ConstructedWithInvalidTransactionId_ThrowsArgumentException()
        {
            // Arrange
            var transactionId = Guid.Empty;
            const string accountId = "12341-1";
            const decimal value = 123_1m;
            var processingDate = DateTime.UtcNow.AddHours(-3);
            const string description = "* IFood";
            const string category = "Food";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Transaction(transactionId, accountId, value, processingDate, description, category));
            
            Assert.Equal("transactionId", exception.ParamName); 
        }
        
        [Fact]
        public void Transaction_ConstructedWithInvalidAccountId_ThrowsArgumentException()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            const string accountId = null;
            const decimal value = 123_1m;
            var processingDate = DateTime.UtcNow.AddHours(-3);
            const string description = "* IFood";
            const string category = "Food";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Transaction(transactionId, accountId, value, processingDate, description, category));
            
            Assert.Equal("accountId", exception.ParamName); 
        }
        
        [Fact]
        public void Transaction_ConstructedWithInvalidProcessingDate_ThrowsArgumentException()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            const string accountId = "1231-1";
            const decimal value = 123_1m;
            var processingDate = DateTime.MinValue;
            const string description = "* IFood";
            const string category = "Food";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new Transaction(transactionId, accountId, value, processingDate, description, category));
            
            Assert.Equal("processingDate", exception.ParamName); 
        }
    }
}