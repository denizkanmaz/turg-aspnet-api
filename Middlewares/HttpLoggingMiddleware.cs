using System.Diagnostics;
using Microsoft.Extensions.Options;
using Turg.App.Services;

namespace Turg.App.Middlewares;

internal class HttpLoggingOptions
{
    public bool LogClientBrowser { get; set; } = false;
}

// factory-based middleware class
internal class HttpLoggingMiddleware : IMiddleware
{
    private readonly ILogger<HttpLoggingMiddleware> _logger;
    private readonly UserActivityService _userActivityService;
    private readonly HttpLoggingOptions _options;

    public HttpLoggingMiddleware(ILogger<HttpLoggingMiddleware> logger, UserActivityService userActivityService, IOptions<HttpLoggingOptions> options)
    {
        _logger = logger;
        _userActivityService = userActivityService;
        _options = options.Value;
        _logger.LogInformation("HttpLoggingMiddleware is created at {Timestamp}", DateTime.UtcNow);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Resolve from DI container
        // var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
        // var userActivityService = context.RequestServices.GetRequiredService<UserActivityService>();

        _userActivityService.StartActivity();

        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;

        _logger.LogInformation("[{Timestamp}] Request: {Method} {Path} from {ClientIP} {ClientBrowser}",
        DateTime.UtcNow,
        request.Method,
        request.Path,
        context.Connection.RemoteIpAddress,
        _options.LogClientBrowser ? request.Headers["User-Agent"] : string.Empty);

        // pre-processing
        await next(context);
        // post-processing

        stopwatch.Stop();

        _logger.LogInformation("[{Timestamp}] Response: {StatusCode} in {ElapsedMilliseconds} ms",
        DateTime.UtcNow,
        context.Response.StatusCode,
        stopwatch.ElapsedMilliseconds);
    }
}