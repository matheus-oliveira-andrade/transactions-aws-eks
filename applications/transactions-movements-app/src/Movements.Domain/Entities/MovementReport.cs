using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Movements.Domain.Entities;

    public class MovementReport
    {
        public decimal Balance { get; }
        public decimal TotalReceived { get; }
        public decimal TotalSpent { get; }
        public Dictionary<string, decimal> SpentByCategories { get; }
        public Dictionary<string, decimal> BalanceByMonths { get; }
        
        
        public MovementReport(List<Movement> movements)
        {
            if (!movements.Any())
                throw new ArgumentException("Cannot be null or empty", nameof(movements));

            if (movements.Any(m => m == null))
                throw new ArgumentException("Cannot contain null items", nameof(movements));
            
            Balance = CalculateBalance(movements);
            TotalReceived = CalculateTotalReceived(movements);
            TotalSpent = CalculateTotalSpent(movements);
            SpentByCategories = CalculateSpentByCategories(movements);
            BalanceByMonths = CalculateBalanceByMonths(movements);
        }

        
        private static Dictionary<string, decimal> CalculateBalanceByMonths(List<Movement> movements)
        {
            return movements
                .GroupBy(m => m.Date.ToString("MMMM", CultureInfo.InvariantCulture))
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
        }

        private static Dictionary<string, decimal> CalculateSpentByCategories(List<Movement> movements)
        {
            return movements
                .GroupBy(m => m.Category)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Value));
        }

        private static decimal CalculateTotalSpent(List<Movement> movements)
        {
            return movements
                .Where(x => x.Value < 0)
                .Sum(m => m.Value);
        }

        private static decimal CalculateTotalReceived(List<Movement> movements)
        {
            return movements
                .Where(x => x.Value > 0)
                .Sum(m => m.Value);
        }

        private static decimal CalculateBalance(List<Movement> movements)
        {
            return movements
                .Sum(m => m.Value);
        }
    }