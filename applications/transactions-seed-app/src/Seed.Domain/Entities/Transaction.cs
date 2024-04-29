using System;

namespace Seed.Domain.Entities
{
    public class Transaction
    {
        public Guid TransactionId { get; private set; }
        public string AccountId { get; private set; }
        public decimal Value { get; private set; }
        public DateTime ProcessingDate { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }

        public Transaction(
            Guid transactionId,
            string accountId,
            decimal value,
            DateTime processingDate,
            string description,
            string category)
        {
            if (transactionId == Guid.Empty)
            {
                throw new ArgumentException("Transaction ID cannot be empty.", nameof(transactionId));
            }

            if (string.IsNullOrWhiteSpace(accountId))
            {
                throw new ArgumentException("Account ID cannot be null or whitespace.", nameof(accountId));
            }

            if (processingDate == DateTime.MinValue || processingDate == DateTime.MaxValue)
            {
                throw new ArgumentException(
                    $"Value can not be equals {DateTime.MinValue} or {DateTime.MaxValue}", nameof(processingDate));
            }

            TransactionId = transactionId;
            AccountId = accountId;
            Value = value;
            ProcessingDate = processingDate;
            Description = description;
            Category = category;
        }
    }
}