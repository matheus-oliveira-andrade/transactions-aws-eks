using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Seed.Domain.Interfaces.Repositories;
using Seed.Infrastructure.Data.Interfaces;
using Seed.Infrastructure.Data.Models;
using Seed.Infrastructure.Data.Repositories;
using Xunit;

namespace Seed.Infrastructure.Tests.Data.Repositories
{
    public class LocalFileTransactionRepositoryTests
    {
        private readonly Mock<ITransactionsDb> _transactionsDbMock;

        private readonly ITransactionRepository _repository;

        public LocalFileTransactionRepositoryTests()
        {
            _transactionsDbMock = new Mock<ITransactionsDb>();

            _repository = new LocalFileTransactionRepository(_transactionsDbMock.Object);
        }
        
        [Fact]
        public async Task GetByRandomAccountIdAsync_ReturnsTransactionsWithSameRandomAccountId()
        {
            // Arrange
            var transactions = new List<TransactionModel>
            {
                new() {AccountId = "1", TransactionId = Guid.NewGuid(), ProcessingDate = DateTime.UtcNow},
                new() {AccountId = "2", TransactionId = Guid.NewGuid(), ProcessingDate = DateTime.UtcNow},
                new() {AccountId = "1", TransactionId = Guid.NewGuid(), ProcessingDate = DateTime.UtcNow},
                new() {AccountId = "3", TransactionId = Guid.NewGuid(), ProcessingDate = DateTime.UtcNow}
            };
            
            _transactionsDbMock
                .Setup(x => x.ReadTransactionsAsync())
                .ReturnsAsync(transactions);
            
            // Act
            var result = await _repository.GetByRandomAccountIdAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(transactions.Count(x => x.AccountId == result.First().AccountId), result.Count);
        }
    }
}