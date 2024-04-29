using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Seed.Application;
using Seed.Infrastructure;
using Serilog;
using Serilog.Formatting.Json;

namespace Seed.Console
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {   
        private const string ApplicationName = "Transactions.Seed";

        private static IHost BuildHost(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                services.AddHostedService<PublishTransactionsHostedService>();
                
                services.AddApplication();
                services.AddInfrastructure(ctx.Configuration);
            })
            .UseSerilog((_, _, loggerConfiguration) => loggerConfiguration
                .Enrich.FromLogContext()
                .Enrich.WithProperty(nameof(ApplicationName), ApplicationName)
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.File(new JsonFormatter(), $"/logs/{ApplicationName.Replace(".", "_")}.log"))
            .Build();

        public static int Main(string[] args)
        {
            BuildHost(args).Run();
            return 0;
        }
    }
}