using Microsoft.Extensions.Caching.Memory;
using Turg.App.Utils;

namespace Turg.App.Filters;

internal class CachingEndpointFilter(IMemoryCache memoryCache) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var cacheKey = CacheKeyUtils.GenerateCacheKey(context.HttpContext.Request);

        if (memoryCache.TryGetValue(cacheKey, out IResult cachedResult))
        {
            context.HttpContext.Response.Headers.Append("X-Cache-Hit", "yes");
            return cachedResult;
        }

        // Pre-process
        var result = await next(context);
        // Post-process

        if (result is IResult httpResult)
        {
            memoryCache.Set(cacheKey, httpResult, new TimeSpan(0, 0, 30));
        }

        return result;
    }
}

internal static class CachingEndpointFilterExtensions
{
    internal static RouteHandlerBuilder WithCaching(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<CachingEndpointFilter>();
    }
}
