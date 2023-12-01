namespace Infrastructure.Service;

/// <summary>
/// Provides an interface for cache operations.
/// </summary>
public interface ICacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
    Task<string> GetCachedResponse(string cacheKey);
}
