using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Turg.App.Filters;

internal class CachingFilter : IResourceFilter
{
    private readonly IMemoryCache _memoryCache;
    private readonly TimeSpan _ttl = new TimeSpan(0, 0, 30);
    private readonly ILogger<CachingFilter> _logger;

    public CachingFilter(IMemoryCache memoryCache, ILogger<CachingFilter> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        _logger.LogInformation("OnResourceExecuting");

        var cacheKey = GenerateCacheKey(context.HttpContext.Request);

        if (_memoryCache.TryGetValue(cacheKey, out IActionResult cachedResult))
        {
            context.Result = cachedResult;
            context.HttpContext.Response.Headers.Append("X-Cache-Hit", "yes");
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        _logger.LogInformation("OnResourceExecuted");

        if (context.HttpContext.Response.StatusCode != 200)
        {
            return;
        }

        var cacheKey = GenerateCacheKey(context.HttpContext.Request);

        _memoryCache.Set(cacheKey, context.Result, _ttl);
    }

    private string GenerateCacheKey(HttpRequest request)
    {
        var stopwatch = Stopwatch.StartNew();

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

            stopwatch.Stop();

            _logger.LogInformation($"Generated cache key: {hashedKey} - {stopwatch.ElapsedMilliseconds}ms");

            return hashedKey;
        }
    }
}
