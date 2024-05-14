using System;
using MassTransit;
using MassTransit.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movements.Application;
using Movements.AsyncMessageReceiver.Consumers;
using Movements.AsyncMessageReceiver.Options;
using Movements.Infrastructure;
using Movements.Infrastructure.Data;
using RabbitMQ.Client;
using Serilog;
using Serilog.Formatting.Json;

namespace Movements.AsyncMessageReceiver;

public static class Program
{
    private const string ApplicationName = "Transactions.Movements.AsyncReceiver";
    
    private static IHost BuildHost(string[] args) => Host.CreateDefaultBuilder(args)
        .ConfigureServices((ctx, services) =>
        {
            services.AddApplication();
            services.AddInfrastructure(ctx.Configuration);
            
            services.AddMassTransit(x =>
            {
                x.AddConsumer<TransactionProcessedConsumer>();
                
                x.UsingRabbitMq((rMqCtx, rMqCfg) =>
                {

                    var rabbitMqSection = ctx.Configuration.GetSection(nameof(RabbitMqOptions));

                    rMqCfg.Host(rabbitMqSection[nameof(RabbitMqOptions.HostName)], h =>
                    {
                        h.Username(rabbitMqSection[nameof(RabbitMqOptions.UserName)]);
                        h.Password(rabbitMqSection[nameof(RabbitMqOptions.Password)]);
                    });
                    
                    rMqCfg.ReceiveEndpoint("transaction-consumer-queue", e =>
                    {
                        e.UseNewtonsoftRawJsonDeserializer(RawSerializerOptions.AddTransportHeaders | RawSerializerOptions.CopyHeaders);

                        e.Bind("processed-transactions", bindCfg =>
                        {
                            bindCfg.Durable = true;
                            bindCfg.AutoDelete = false;
                            bindCfg.ExchangeType = ExchangeType.Fanout;
                            bindCfg.RoutingKey = "processed_transaction";
                        });
                        
                        e.RethrowFaultedMessages();
                        e.ConfigureConsumer<TransactionProcessedConsumer>(rMqCtx, cCfg =>
                        {
                            cCfg.ConcurrentMessageLimit = 1;
                        });
                        e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(60)));
                    });

                    rMqCfg.ConfigureEndpoints(rMqCtx);
                });
            });
        })
        .UseSerilog((_, _, loggerConfiguration) => loggerConfiguration
            .Enrich.FromLogContext()
            .Enrich.WithProperty(nameof(ApplicationName), ApplicationName)
            .WriteTo.Console(new JsonFormatter())
            .WriteTo.File(new JsonFormatter(), $"/logs/{ApplicationName.Replace(".", "_")}.log")
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning))
        .Build();

    public static int Main(string[] args)
    {
        BuildHost(args)
            .ApplyDbMigrations()
            .Run();
        return 0;
    }

    private static IHost ApplyDbMigrations(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        
        var db = scope.ServiceProvider.GetRequiredService<MovementsDbContext>();
        db.Database.Migrate();

        return host;
    }
}