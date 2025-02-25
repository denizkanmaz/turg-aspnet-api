using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Turg.App.Models;

namespace Turg.App.Filters;

internal class CachingEndpointFilter : IEndpointFilter
{
    private readonly IMemoryCache _memoryCache;
    public CachingEndpointFilter(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        Console.WriteLine("CachingEndpointFilter.ctor");
    }
    
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        Console.WriteLine("CachingEndpointFilter.InvokeAsync");
        var cacheKey = GenerateCacheKey(context.HttpContext.Request);

        if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Product> cachedResult))
        {
            context.HttpContext.Response.Headers.Append("X-Cache-Hit", "yes");
            return cachedResult;
        }

        // Pre-process
        var result = await next(context);
        // Post-process

        _memoryCache.Set(cacheKey, result, new TimeSpan(0, 0, 30));
        return result;
    }

    private static string GenerateCacheKey(HttpRequest request)
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
