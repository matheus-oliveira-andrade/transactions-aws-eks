using System.Collections.Generic;
using Movements.Domain.Entities;

namespace Movements.Api.Models.Report.Responses;

public class MovementReportResponse
{
    public decimal Balance { get; set; }
    public decimal TotalReceived { get; set; }
    public decimal TotalSpent { get; set; }
    public Dictionary<string, decimal> SpentByCategories { get; set; }
    public Dictionary<string, decimal> BalanceByMonths { get; set; }
}

public static class MovementReportResponseMapperExtension
{
    public static MovementReportResponse ToResponse(this MovementReport movementReport) => new()
    {
        Balance = movementReport.Balance,
        TotalReceived = movementReport.TotalReceived,
        TotalSpent = movementReport.TotalSpent,
        SpentByCategories = movementReport.SpentByCategories,
        BalanceByMonths = movementReport.BalanceByMonths
    };
}