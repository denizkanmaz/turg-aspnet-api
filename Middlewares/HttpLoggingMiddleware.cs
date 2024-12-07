using System.Diagnostics;
using Turg.App.Services;

namespace Turg.App.Middlewares;

// conventional middleware class
internal class HttpLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpLoggingMiddleware> _logger;

    public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        _logger.LogInformation("HttpLoggingMiddleware is created at {Timestamp}", DateTime.UtcNow);
    }

    // public void Invoke(HttpContext context)
    // {
    //     // sync logic
    // }

    public async Task InvokeAsync(HttpContext context, UserActivityService userActivityService)
    {
        // Resolve from DI container
        // var logger = context.RequestServices.GetRequiredService<ILogger<Startup>>();
        // var userActivityService = context.RequestServices.GetRequiredService<UserActivityService>();

        userActivityService.StartActivity();

        var stopwatch = Stopwatch.StartNew();

        var request = context.Request;

        _logger.LogInformation("[{Timestamp}] Request: {Method} {Path} from {ClientIP}",
        DateTime.UtcNow,
        request.Method,
        request.Path,
        context.Connection.RemoteIpAddress);

        // pre-processing
        await _next(context);
        // post-processing

        stopwatch.Stop();

        _logger.LogInformation("[{Timestamp}] Response: {StatusCode} in {ElapsedMilliseconds} ms",
        DateTime.UtcNow,
        context.Response.StatusCode,
        stopwatch.ElapsedMilliseconds);
    }
}