using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Turg.App.Utils;

namespace Turg.App.Filters;

internal class CachingFilter(IMemoryCache memoryCache) : IResourceFilter
{
    private static readonly TimeSpan DefaultTTL = new TimeSpan(0, 0, 30);

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var cacheKey = CacheKeyUtils.GenerateCacheKey(context.HttpContext.Request);

        if (memoryCache.TryGetValue(cacheKey, out IActionResult cachedResult))
        {
            context.Result = cachedResult;
            context.HttpContext.Response.Headers.Append("X-Cache-Hit", "yes");
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        if (context.HttpContext.Response.StatusCode != 200)
        {
            return;
        }

        var cacheKey = CacheKeyUtils.GenerateCacheKey(context.HttpContext.Request);

        memoryCache.Set(cacheKey, context.Result, DefaultTTL);
    }
}
