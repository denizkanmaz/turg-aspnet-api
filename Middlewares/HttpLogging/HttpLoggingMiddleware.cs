using System.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using Turg.App.HttpContextExtensions.StorageItemExtensions;
using Turg.App.Middlewares.HttpLogging.Services;
using Turg.App.StorageItems;

namespace Turg.App.Middlewares.HttpLogging;

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

        var httpActivityFeature = context.Features.Get<IHttpActivityFeature>();

        _userActivityService.StartActivity();

        var startedAt = httpActivityFeature is not null ? httpActivityFeature.Activity.StartTimeUtc : DateTime.UtcNow;

        var request = context.Request;

        var clientMetadata = context.GetItem<ClientMetadata>();

        _logger.LogInformation("[{Timestamp}] Request: {Method} {Path} from {ClientIP} {ClientBrowser} {IsMobile}",
        DateTime.UtcNow,
        request.Method,
        request.Path,
        context.Connection.RemoteIpAddress,
        _options.LogClientBrowser ? request.Headers["User-Agent"] : string.Empty,
        clientMetadata?.Device.IsMobile);

        // pre-processing
        await next(context);
        // post-processing

        var endedAt = DateTime.UtcNow;
        var duration = endedAt - startedAt;

        _logger.LogInformation("[{Timestamp}] Response: {StatusCode} in {ElapsedMilliseconds} ms",
        DateTime.UtcNow,
        context.Response.StatusCode,
        duration.TotalMilliseconds);
    }
}