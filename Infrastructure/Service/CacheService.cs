using System.Text.Json;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Service;

/// <summary>
/// Provides caching services using Redis. This service allows caching of responses
/// and retrieving them using a cache key.
/// </summary>
public class CacheService : ICacheService
{
    private readonly IDatabase _database;
    private readonly ILogger<CacheService> _logger;

    /// <summary>
    /// Initializes a new instance of the CacheService with a Redis database and a logger.
    /// </summary>
    /// <param name="redis">The connection multiplexer for accessing the Redis server.</param>
    /// <param name="logger">The logger for logging errors and information.</param>
    public CacheService(IConnectionMultiplexer redis, ILogger<CacheService> logger)
    {
        _database = redis.GetDatabase();
        _logger = logger;
    }

    /// <summary>
    /// Caches the response object for the specified duration.
    /// </summary>
    /// <param name="cacheKey">The key used for caching the response.</param>
    /// <param name="response">The response object to cache.</param>
    /// <param name="timeToLive">The duration for which the response should be cached.</param>
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    {
        if (response is null)
        {
            return;
        }
        try 
        {
            var options = new JsonSerializerOptions 
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(response, options);
            await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "We have encountered an error on our cache response");
        }
    }

    /// <summary>
    /// Retrieves a cached response using the specified cache key.
    /// </summary>
    /// <param name="cacheKey">The key used to retrieve the cached response.</param>
    /// <returns>A cached response if available, or an empty string if not.</returns>
    public async Task<string> GetCachedResponse(string cacheKey)
    {
        var cachedResponse = await _database.StringGetAsync(cacheKey);

        return cachedResponse.HasValue ? cachedResponse.ToString() : string.Empty;
    }
}
