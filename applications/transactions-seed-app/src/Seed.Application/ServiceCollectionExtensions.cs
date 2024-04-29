using Microsoft.Extensions.DependencyInjection;
using Seed.Application.Events;

namespace Seed.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(QueueTransactionsEvent).Assembly));
        }
    }
}