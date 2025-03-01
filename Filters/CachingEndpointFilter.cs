using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Turg.App.Filters;

internal class CachingEndpointFilter(IMemoryCache memoryCache) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var cacheKey = GenerateCacheKey(context.HttpContext.Request);

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

internal static class CachingEndpointFilterExtensions
{
    internal static RouteHandlerBuilder WithCaching(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<CachingEndpointFilter>();
    }
}
