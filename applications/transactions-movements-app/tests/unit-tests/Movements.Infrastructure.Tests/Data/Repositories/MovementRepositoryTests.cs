using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using Movements.Domain.Entities;
using Movements.Infrastructure.Data;
using Movements.Infrastructure.Data.Models;
using Movements.Infrastructure.Data.Repositories;
using Xunit;

namespace Movements.Infrastructure.Tests.Data.Repositories
{
    public class MovementRepositoryTests
    {
        private readonly Mock<MovementsDbContext> _movementDbContextMock;
        
        private readonly MovementRepository _movementRepository;
        private readonly Mock<DbSet<MovementModel>> _dbSetMovementMock;

        public MovementRepositoryTests()
        {
            _dbSetMovementMock = new Mock<DbSet<MovementModel>>();
            
            _movementDbContextMock = new Mock<MovementsDbContext>();
            _movementDbContextMock
                .Setup(x => x.Movements)
                .Returns(_dbSetMovementMock.Object);

            _movementRepository = new MovementRepository(_movementDbContextMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAdd_WithSuccess()
        {
            // Arrange
            var movement = new Movement(
                Guid.NewGuid(),
                "1231-1",
                DateTime.Today,
                56_00,
                "",
                "");
            
            // Act
            await _movementRepository.AddAsync(movement);

            // Assert
            _dbSetMovementMock.Verify(x => x.Add(It.IsAny<MovementModel>()), Times.Once);
            _movementDbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));
        }
    }
}