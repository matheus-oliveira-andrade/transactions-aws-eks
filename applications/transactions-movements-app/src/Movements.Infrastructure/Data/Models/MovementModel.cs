using System;
using Movements.Domain.Entities;

namespace Movements.Infrastructure.Data.Models
{
    public class MovementModel
    {
        public Guid Id { get; set; }
        public Guid TransactionId { get; set; }
        public string AccountId { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
    }

    public static class MovementModelMapperExtensions
    {
        public static MovementModel ToModel(this Movement movement) => new()
        {
            Id = movement.Id,
            TransactionId = movement.TransactionId,
            AccountId = movement.AccountId,
            Date = movement.Date,
            Value = movement.Value,
            Category = movement.Category,
            Description = movement.Description
        };

        public static Movement FromEntity(this MovementModel model) => new(
            model.TransactionId,
            model.AccountId,
            model.Date,
            model.Value,
            model.Category,
            model.Description,
            model.Id);
    }
}