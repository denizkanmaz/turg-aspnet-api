using System.Diagnostics;
using Turg.App.Services;

namespace Turg.App.Middlewares;

// factory-based middleware class
internal class HttpLoggingMiddleware : IMiddleware
{
    private readonly ILogger<HttpLoggingMiddleware> _logger;
    private readonly UserActivityService _userActivityService;

    public HttpLoggingMiddleware(ILogger<HttpLoggingMiddleware> logger, UserActivityService userActivityService)
    {
        _logger = logger;
        _userActivityService = userActivityService;
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

        _logger.LogInformation("[{Timestamp}] Request: {Method} {Path} from {ClientIP}",
        DateTime.UtcNow,
        request.Method,
        request.Path,
        context.Connection.RemoteIpAddress);

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