using System.Collections.Generic;
using System.Threading.Tasks;
using Seed.Domain.Entities;

namespace Seed.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetByRandomAccountIdAsync();
    }
}