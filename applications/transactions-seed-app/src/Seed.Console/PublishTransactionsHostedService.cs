using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Seed.Application.Events;

namespace Seed.Console
{
    [ExcludeFromCodeCoverage]
    public class PublishTransactionsHostedService : BackgroundService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PublishTransactionsHostedService> _logger;

        public PublishTransactionsHostedService(IMediator mediator, ILogger<PublishTransactionsHostedService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _mediator.Send(new QueueTransactionsEvent(), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error queueing transactions");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}