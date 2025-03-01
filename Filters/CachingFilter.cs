using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Turg.App.Filters;

internal class CachingFilter(IMemoryCache memoryCache) : IResourceFilter
{
    private readonly TimeSpan _ttl = new TimeSpan(0, 0, 30);


    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var cacheKey = GenerateCacheKey(context.HttpContext.Request);

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

        var cacheKey = GenerateCacheKey(context.HttpContext.Request);

        memoryCache.Set(cacheKey, context.Result, _ttl);
    }

    private string GenerateCacheKey(HttpRequest request)
    {
        var queries = request.Query.OrderBy(x => x.Key);

        var keyBuilder = new StringBuilder();
        keyBuilder.Append(request.Path);

        foreach (var query in queries)
        {
            keyBuilder.Append($"|{query.Key}-{query.Value}");
        }

        using (var md5 = MD5.Create())
        {
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(keyBuilder.ToString()));
            var hashedKey = Convert.ToBase64String(hashBytes);

            return hashedKey;
        }
    }
}
