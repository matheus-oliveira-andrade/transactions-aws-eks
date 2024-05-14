using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Movements.Application.Queries;
using Movements.Domain.Entities;
using Movements.Domain.Interfaces.Data;

namespace Movements.Application.QueryHandlers;

public class GetAccountMovementsReportQueryHandler : IRequestHandler<GetAccountMovementsReportQuery, MovementReport>
{
    private readonly ILogger<GetAccountMovementsReportQueryHandler> _logger;
    private readonly IMovementRepository _movementRepository;

    public GetAccountMovementsReportQueryHandler(
        ILogger<GetAccountMovementsReportQueryHandler> logger,
        IMovementRepository movementRepository)
    {
        _logger = logger;
        _movementRepository = movementRepository;
    }

    public async Task<MovementReport> Handle(GetAccountMovementsReportQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting movements report for account {@AccountId}", request.AccountId);

        var movements = await _movementRepository.GetByAccountIdAsync(request.AccountId);

        return movements.Any()
            ? new MovementReport(movements)
            : null;
    }
}