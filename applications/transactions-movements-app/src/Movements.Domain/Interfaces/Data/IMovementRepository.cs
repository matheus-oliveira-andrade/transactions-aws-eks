using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Movements.Domain.Entities;

namespace Movements.Domain.Interfaces.Data
{
    public interface IMovementRepository
    {
        Task AddAsync(Movement movement, CancellationToken cancellationToken = default);

        Task<List<Movement>> GetByAccountIdAsync(string accountId);
    }
}