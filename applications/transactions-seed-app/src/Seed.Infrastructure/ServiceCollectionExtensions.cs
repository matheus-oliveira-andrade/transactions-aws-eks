using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seed.Domain.Interfaces.Bus;
using Seed.Domain.Interfaces.Repositories;
using Seed.Infrastructure.Bus;
using Seed.Infrastructure.Bus.Interfaces;
using Seed.Infrastructure.Bus.Options;
using Seed.Infrastructure.Data;
using Seed.Infrastructure.Data.Interfaces;
using Seed.Infrastructure.Data.Repositories;

namespace Seed.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddData();
            
            services.AddBus(configuration);
        }

        private static void AddData(this IServiceCollection services)
        {
            services.AddTransient<ITransactionsDb, TransactionsDb>();
            services.AddTransient<ITransactionRepository, LocalFileTransactionRepository>();
        }
        
        private static void AddBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IRabbitMqConnection, RabbitMqConnection>();
            services.AddTransient<IBusService, RabbitMqBusService>();
            
            services.Configure<RabbitMqOptions>(opt =>
            {
                var rabbitMqSection = configuration.GetSection(nameof(RabbitMqOptions));
                
                opt.HostName = rabbitMqSection[nameof(RabbitMqOptions.HostName)];
                Console.WriteLine(rabbitMqSection[nameof(RabbitMqOptions.HostName)]);
                
                opt.UserName = rabbitMqSection[nameof(RabbitMqOptions.UserName)];
                Console.WriteLine(rabbitMqSection[nameof(RabbitMqOptions.UserName)]);
                
                opt.Password = rabbitMqSection[nameof(RabbitMqOptions.Password)];
                Console.WriteLine(rabbitMqSection[nameof(RabbitMqOptions.Password)]);
            });
        }
    }
}