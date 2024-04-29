using System;
using Movements.Domain.Entities;
using Movements.Infrastructure.Data.Models;
using Xunit;

namespace Movements.Infrastructure.Tests.Data.Models
{
    public class MovementModelTests
    {
        [Fact]
        public void ToModel_ShouldConvertEntityToModel_WithSuccess()
        {
            // Arrange
            var entity = new Movement(
                Guid.NewGuid(),
                "1346-5",
                DateTime.Today,
                10_89m,
                "Educação",
                "");

            // Act
            var result = entity.ToModel();

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(result.Id, Guid.Empty);
            Assert.Equal(entity.TransactionId, result.TransactionId);
            Assert.Equal(entity.AccountId, result.AccountId);
            Assert.Equal(entity.Date, result.Date);
            Assert.Equal(entity.Value, result.Value);
            Assert.Equal(entity.Category, result.Category);
            Assert.Equal(entity.Description, result.Description);
        }
        
        [Fact]
        public void FromEntity_ShouldConvertModelToEntity_WithSuccess()
        {
            // Arrange
            var model = new MovementModel
            {
                Id = Guid.NewGuid(),
                AccountId = "1346-5",
                TransactionId = Guid.NewGuid(),
                Date = DateTime.Today,
                Value = 10_89m,
                Category = "Educação",
                Description = ""
            };

            // Act
            var result = model.FromEntity();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(model.Id, result.Id);
            Assert.Equal(model.TransactionId, result.TransactionId);
            Assert.Equal(model.AccountId, result.AccountId);
            Assert.Equal(model.Date, result.Date);
            Assert.Equal(model.Value, result.Value);
            Assert.Equal(model.Category, result.Category);
            Assert.Equal(model.Description, result.Description);
        }
    }
}