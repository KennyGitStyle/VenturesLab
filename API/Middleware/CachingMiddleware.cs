using API.Helper;
using StackExchange.Redis;

namespace API.Middleware;

/// <summary>
/// Middleware for caching responses in an ASP.NET Core application.
/// This class intercepts HTTP requests and responses to implement caching logic using Redis.
/// </summary>
public class CachingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDatabase _cache;

    public CachingMiddleware(RequestDelegate next, IDatabase cache)
    {
        _next = next;
        _cache = cache;
    }

    /// <summary>
    /// Invokes the middleware to perform caching operations.
    /// </summary>
    /// <param name="context">The HttpContext for the current request.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        var cacheable = context.GetEndpoint()?.Metadata.GetMetadata<CacheableAttribute>();
        if (cacheable is null)
        {
            await _next(context);
            return;
        }
        var cacheKey = GenerateCacheKey(context.Request);
        var cachedResponse = await _cache.StringGetAsync(cacheKey);
        if (!cachedResponse.IsNull)
        {
            // Check if cachedResponse is not null or empty before writing to response
            var responseContent = cachedResponse.HasValue ? cachedResponse.ToString() : string.Empty;
            await context.Response.WriteAsync(responseContent);
            return;
        }

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;
        await _next(context);

        responseBody.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(responseBody).ReadToEndAsync();

        // Check if responseText is not null or empty before caching and writing to response
        if (!string.IsNullOrEmpty(responseText))
        {
            var expiration = TimeSpan.FromSeconds(cacheable.TimeToLiveSeconds);
            await _cache.StringSetAsync(cacheKey, responseText, expiration);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    /// <summary>
    /// Generates a unique cache key based on the request's path and query string.
    /// </summary>
    /// <param name="request">The HTTP request to generate the cache key for.</param>
    private static string GenerateCacheKey(HttpRequest request) 
        => $"{request.Path}_{request.QueryString}";
}
