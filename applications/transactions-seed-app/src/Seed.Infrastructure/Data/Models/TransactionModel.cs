using System;
using System.Collections.Generic;
using System.Linq;
using Seed.Domain.Entities;

namespace Seed.Infrastructure.Data.Models
{
    public class TransactionModel
    {
        public Guid TransactionId { get; set; }
        public string AccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }

    public static class TransactionModelExtension
    {
        public static Transaction ToEntity(this TransactionModel model) => new Transaction(
                model.TransactionId,
                model.AccountId,
                model.Value,
                model.ProcessingDate,
                model.Description,
                model.Category);

        public static IEnumerable<Transaction> ToEntities(this IEnumerable<TransactionModel> models) => 
            models.Select(m => m.ToEntity());
    }
}