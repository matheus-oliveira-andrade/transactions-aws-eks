using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Seed.Application.Events;
using Seed.Domain.Interfaces.Bus;
using Seed.Domain.Interfaces.Repositories;

namespace Seed.Application.EventHandlers
{
    public class QueueTransactionsEventHandler : IRequestHandler<QueueTransactionsEvent>
    {
        private readonly ILogger<QueueTransactionsEventHandler> _logger;
        private readonly IBusService _busService;
        private readonly ITransactionRepository _transactionRepository;

        private const string Exchange = "processed-transactions";
        private const string Key = "processed_transaction";
        
        public QueueTransactionsEventHandler(
            ILogger<QueueTransactionsEventHandler> logger, 
            IBusService busService,
            ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _busService = busService;
            _transactionRepository = transactionRepository;
        }
        
        public async Task Handle(QueueTransactionsEvent request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Reading transactions");
            
            var transactions = await _transactionRepository.GetByRandomAccountIdAsync();

            _logger.LogInformation("Publishing transactions");
            
            var messages = transactions.Select(JsonConvert.SerializeObject).ToList();
            _busService.Publish(Exchange, Key, messages);
            
            _logger.LogInformation("Transactions published");
        }
    }
}