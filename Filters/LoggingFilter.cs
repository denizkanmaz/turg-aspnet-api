using Microsoft.AspNetCore.Mvc.Filters;

namespace Turg.App.Filters;

internal class LoggingFilter : IResourceFilter
{
    private readonly ILogger<LoggingFilter> _logger;
    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
        _logger.LogInformation("ctor");
    }
    
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        var requestId = context.HttpContext.TraceIdentifier;
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var actionName = context.RouteData.Values["action"];
        var controllerName = context.RouteData.Values["controller"];
        var clientIpAddress = context.HttpContext.Connection.RemoteIpAddress;

        var requestLog = $"[{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}] Request: {requestId} {method} {path} ({controllerName} {actionName}) from {clientIpAddress}";

        _logger.LogInformation(requestLog);
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        var requestId = context.HttpContext.TraceIdentifier;
        var statusCode = context.HttpContext.Response.StatusCode;

        var responseLog = $"[{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}] Response: {requestId} {statusCode}";

        _logger.LogInformation(responseLog);
    }
}
