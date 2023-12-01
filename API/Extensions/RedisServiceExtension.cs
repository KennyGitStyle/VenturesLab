using Infrastructure.Service;
using StackExchange.Redis;

namespace API.Extensions;

/// <summary>
/// Provides an extension method for IServiceCollection to configure Redis services.
/// This class is used to integrate Redis into the application's service collection, enabling caching and other Redis-based functionalities.
/// </summary>
public static class RedisServiceExtension
{
    /// <summary>
    /// Adds and configures Redis services, including the connection multiplexer, database, and caching service.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="config">The application's configuration to access the Redis connection string.</param>
    /// <returns>The updated IServiceCollection instance with Redis services added.</returns>
    public static IServiceCollection AddRedisServiceExtension(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConnectionMultiplexer>(p =>
        {
            var redisConnectionString = config.GetConnectionString("Redis");
            if (string.IsNullOrEmpty(redisConnectionString))
            {
                throw new InvalidOperationException("Redis connection string is not configured.");
            }

            var configuration = ConfigurationOptions.Parse(redisConnectionString);
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddSingleton<IDatabase>(p =>
        {
            var multiplexer = p.GetRequiredService<IConnectionMultiplexer>();
            return multiplexer.GetDatabase();
        });

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}
