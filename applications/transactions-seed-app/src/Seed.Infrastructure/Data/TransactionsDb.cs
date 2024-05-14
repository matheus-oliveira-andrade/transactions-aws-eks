using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Seed.Infrastructure.Data.Interfaces;
using Seed.Infrastructure.Data.Models;

namespace Seed.Infrastructure.Data
{
    [ExcludeFromCodeCoverage]
    public class TransactionsDb : ITransactionsDb
    {
        private const string TransactionsFile = "transactions.json";
        
        public async Task<List<TransactionModel>> ReadTransactionsAsync()
        {
            using var streamReader = new StreamReader(TransactionsFile, Encoding.UTF8);

            var jsonData = await streamReader.ReadToEndAsync();

            var deserializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            return JsonSerializer.Deserialize<List<TransactionModel>>(jsonData, deserializerOptions);
        }
    }
}