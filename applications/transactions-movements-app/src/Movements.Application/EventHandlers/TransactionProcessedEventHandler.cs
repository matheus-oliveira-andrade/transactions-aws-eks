using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Movements.Application.Events;
using Movements.Domain.Entities;
using Movements.Domain.Interfaces.Data;

namespace Movements.Application.EventHandlers
{
    public class TransactionProcessedEventHandler : IRequestHandler<TransactionProcessed>
    {
        private readonly ILogger<TransactionProcessedEventHandler> _logger;
        private readonly IMovementRepository _movementRepository;

        public TransactionProcessedEventHandler(
            ILogger<TransactionProcessedEventHandler> logger, 
            IMovementRepository movementRepository)
        {
            _logger = logger;
            _movementRepository = movementRepository;
        }

        public async Task Handle(TransactionProcessed transactionProcessed, CancellationToken cancellationToken)
        {
            _logger.LogInformation("handling transaction processed {@Request}", transactionProcessed);

            var movement = new Movement(
                transactionProcessed.TransactionId,
                transactionProcessed.AccountId,
                transactionProcessed.ProcessingDate,
                transactionProcessed.Value,
                transactionProcessed.Category,
                transactionProcessed.Description);

            _logger.LogInformation("adding new movement {@Movement}", movement);
            
            await _movementRepository.AddAsync(movement, cancellationToken);
        }
    }
}