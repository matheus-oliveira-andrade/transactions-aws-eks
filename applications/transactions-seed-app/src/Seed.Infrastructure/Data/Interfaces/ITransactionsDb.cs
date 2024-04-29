using System.Collections.Generic;
using System.Threading.Tasks;
using Seed.Infrastructure.Data.Models;

namespace Seed.Infrastructure.Data.Interfaces
{
    public interface ITransactionsDb
    {
        Task<List<TransactionModel>> ReadTransactionsAsync();
    }
}