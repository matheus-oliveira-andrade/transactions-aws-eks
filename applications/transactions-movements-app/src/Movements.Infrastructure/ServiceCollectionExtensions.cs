using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movements.Domain.Interfaces.Data;
using Movements.Infrastructure.Data;
using Movements.Infrastructure.Data.Repositories;

namespace Movements.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddData(configuration);
        }

        private static void AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IMovementRepository, MovementRepository>();

            services.AddDbContext<MovementsDbContext>(options =>
            {
                options.UseNpgsql(BuildConnectionString(configuration));
            });
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            var connectionDbSection = configuration.GetSection("MovementsDb");

            return $"User ID={connectionDbSection["User"]};Password={connectionDbSection["Password"]};Host={connectionDbSection["Host"]};Port=5432;Database={connectionDbSection["Host"]};Pooling=true;";
        }
    }
}