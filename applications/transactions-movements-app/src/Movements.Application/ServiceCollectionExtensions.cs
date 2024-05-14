using Microsoft.Extensions.DependencyInjection;
using Movements.Application.Events;

namespace Movements.Application
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(TransactionProcessed).Assembly));
        }
    }
}