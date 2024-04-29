using System;
using MediatR;

namespace Movements.Application.Events
{
    public class TransactionProcessed : IRequest
    {
        public Guid TransactionId { get; set; }
        public string AccountId { get; set; }
        public decimal Value { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}