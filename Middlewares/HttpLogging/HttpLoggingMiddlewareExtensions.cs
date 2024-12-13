using Turg.App.Middlewares.HttpLogging.Services;

namespace Turg.App.Middlewares.HttpLogging;

internal static class HttpLoggingMiddlewareExtensions
{
    /// <summary>
    /// A marker class to determine if <see cref="AddCustomHttpLogging(IServiceCollection, IConfiguration)"/> is called in ConfigureServices.
    /// </summary>
    private class HttpLoggingServiceMarker
    { }

    internal static IServiceCollection AddCustomHttpLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HttpLoggingOptions>(configuration.GetSection("HttpLogging"));
        services.AddScoped<HttpLoggingMiddleware>();
        services.AddScoped<UserActivityService>();

        // This service must be the last one to register.
        services.AddSingleton<HttpLoggingServiceMarker>();

        return services;
    }

    internal static IApplicationBuilder UseCustomHttpLogging(this IApplicationBuilder app)
    {
        var markerService = app.ApplicationServices.GetService<HttpLoggingServiceMarker>();

        if (markerService == null)
        {
            throw new InvalidOperationException("Cannot find the required services. Please call AddCustomHttpLogging in ConfigureServices");
        }

        return app.UseMiddleware<HttpLoggingMiddleware>();
    }
}
