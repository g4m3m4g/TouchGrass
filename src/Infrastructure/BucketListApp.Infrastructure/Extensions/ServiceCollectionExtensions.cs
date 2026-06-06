using BucketListApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BucketListApp.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Infrastructure layer services to the dependency injection container.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext with Oracle
        var connectionString = configuration.GetConnectionString("OracleConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseOracle(connectionString));

        return services;
    }
}
