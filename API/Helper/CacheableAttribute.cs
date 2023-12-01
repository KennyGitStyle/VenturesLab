using System.Text;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace API.Helper;

/// <summary>
/// Custom attribute to enable caching for an action method within an MVC controller.
/// It implements the IAsyncActionFilter to intercept action executions for caching purposes.
/// </summary>
public class CacheableAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeToLiveSeconds;

    public CacheableAttribute(int timeToLiveSeconds)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    public int TimeToLiveSeconds => _timeToLiveSeconds;

    /// <summary>
    /// Intercepts action executions to implement caching logic.
    /// </summary>
    /// <param name="context">The context before the action executes.</param>
    /// <param name="next">The delegate to execute the next action filter or the action itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var cachedResponse = await cacheService.GetCachedResponse(cacheKey);

        if (!string.IsNullOrEmpty(cachedResponse))
        {
            context.Result = new ContentResult
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = 200
            };

            return;
        }

        var executedContext = await next();

        if (executedContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
        { 
            await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
        }
    }

    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder(request.Path);

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }

        return keyBuilder.ToString();
    }
}