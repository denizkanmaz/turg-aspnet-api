namespace Turg.App.Middlewares;

// convention-based middleware class
internal class BlacklistingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<BlacklistingMiddleware> _logger;
    
    private readonly List<string> _blacklistedIps = [
        "123.13.13.13",
        "125.15.15.15",
        "126.16.16.16"
    ];

    public BlacklistingMiddleware(RequestDelegate next, ILogger<BlacklistingMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        _logger.LogInformation("BlacklistingMiddleware is created at {Timestamp}", DateTime.UtcNow);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress.ToString();

        if (_blacklistedIps.Contains(clientIp))
        {
            _logger.LogWarning("Blacklisted client is blocked: {ClientIP}", clientIp);

            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            return;
        }

        await _next(context);
    }
}
