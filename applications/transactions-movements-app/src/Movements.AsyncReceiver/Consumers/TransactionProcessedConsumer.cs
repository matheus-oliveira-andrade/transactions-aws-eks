using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Movements.Application.Events;

namespace Movements.AsyncMessageReceiver.Consumers
{
    public class TransactionProcessedConsumer : IConsumer<TransactionProcessed>
    {
        private readonly ILogger<TransactionProcessedConsumer> _logger;
        private readonly IMediator _mediator;

        public TransactionProcessedConsumer(
            ILogger<TransactionProcessedConsumer> logger,
            IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<TransactionProcessed> context)
        {
            _logger.LogInformation("Received transaction processed");
            
            var timer = Stopwatch.StartNew();
            
            try
            {
                await _mediator.Send(context.Message);
            }
            catch (Exception ex)
            {
                await context.NotifyFaulted(timer.Elapsed, nameof(TransactionProcessedConsumer), ex);
            }
        }
    }
}