using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seed.Domain.Entities;
using Seed.Domain.Extensions;
using Seed.Domain.Interfaces.Repositories;
using Seed.Infrastructure.Data.Interfaces;
using Seed.Infrastructure.Data.Models;

namespace Seed.Infrastructure.Data.Repositories
{
    public class LocalFileTransactionRepository : ITransactionRepository
    {
        private readonly ITransactionsDb _transactionsDb;

        public LocalFileTransactionRepository(ITransactionsDb transactionsDb)
        {
            _transactionsDb = transactionsDb;
        }
        
        public async Task<List<Transaction>> GetByRandomAccountIdAsync()
        {
            var transactions = await _transactionsDb.ReadTransactionsAsync();

            var randAccountId = transactions.Shuffle().First().AccountId;
            
            return transactions
                .Where(s => s.AccountId == randAccountId)
                .ToEntities()
                .ToList();
        }
    }
}