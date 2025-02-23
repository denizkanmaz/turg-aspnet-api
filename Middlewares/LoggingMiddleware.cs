
namespace Turg.App.Middleware;

internal class LoggingMiddleware(ILogger<LoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestId = context.TraceIdentifier;
        var method = context.Request.Method;
        var path = context.Request.Path;
        var clientIpAddress = context.Connection.RemoteIpAddress;

        var requestLog = $"[{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}] Request: {requestId} {method} {path} from {clientIpAddress}";

        logger.LogInformation(requestLog);

        // Pre-processing
        await next(context);
        // Post-processing

        var statusCode = context.Response.StatusCode;
        var responseLog = $"[{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}] Response: {requestId} {statusCode}";

        logger.LogInformation(responseLog);
    }
}
