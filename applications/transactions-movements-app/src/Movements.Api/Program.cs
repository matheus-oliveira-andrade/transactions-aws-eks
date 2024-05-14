using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Movements.Application;
using Movements.Infrastructure;
using Serilog;
using Serilog.Formatting.Json;

namespace Movements.Api;

public partial class Program
{
    public static void Main(string[] args)
    {
        const string applicationName = "Transactions.Movements.Api";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHealthChecks();
        
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .Enrich.FromLogContext()
            .Enrich.WithProperty(nameof(applicationName), applicationName)
            .WriteTo.Console()
            .WriteTo.File(new JsonFormatter(), $"/logs/{applicationName.Replace(".", "_")}.log")
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning));

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApplication();

        builder.Services.AddControllers();

        builder.Services.AddApiVersioning();
        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        var app = builder.Build();

        app.MapHealthChecks("/healthz");

        app.UsePathBase("/movements");

        app.UseSwagger();
        app.UseSwaggerUI(opt => opt.SwaggerEndpoint("/movements/swagger/v1/swagger.json", "Swagger V1"));

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}