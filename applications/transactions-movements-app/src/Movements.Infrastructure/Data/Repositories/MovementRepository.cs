using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movements.Domain.Entities;
using Movements.Domain.Interfaces.Data;
using Movements.Infrastructure.Data.Models;

namespace Movements.Infrastructure.Data.Repositories
{
    public class MovementRepository : IMovementRepository
    {
        private readonly MovementsDbContext _movementsDbContext;

        public MovementRepository(MovementsDbContext movementsDbContext)
        {
            _movementsDbContext = movementsDbContext;
        }

        public async Task AddAsync(Movement movement, CancellationToken cancellationToken = default)
        {
            _movementsDbContext.Movements.Add(movement.ToModel());
            await _movementsDbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task<List<Movement>> GetByAccountIdAsync(string accountId)
        {
            return await _movementsDbContext.Movements
                .Where(x => x.AccountId == accountId)
                .Select(x => x.FromEntity())
                .ToListAsync();
        }
    }
    
    
}