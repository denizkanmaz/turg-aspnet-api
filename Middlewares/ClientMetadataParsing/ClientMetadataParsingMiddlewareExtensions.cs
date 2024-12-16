using UAParser;

namespace Turg.App.Middlewares.ClientMetadataParsing;

internal static class ClientMetadataParsingExtensions
{
    /// <summary>
    /// A marker class ...
    /// </summary>
    private class ClientMetadataParsingServiceMarker { }

    internal static IServiceCollection AddClientMetadataParsing(this IServiceCollection services)
    {
        services.AddScoped<ClientMetadataParsingMiddleware>();
        services.AddSingleton(Parser.GetDefault());

        // Must be the last one to register.
        services.AddSingleton<ClientMetadataParsingServiceMarker>();

        return services;
    }

    internal static IApplicationBuilder UseClientMetadataParsing(this IApplicationBuilder app)
    {
        var marker = app.ApplicationServices.GetService<ClientMetadataParsingServiceMarker>();

        if (marker == null)
        {
            throw new InvalidOperationException("Cannot find the required services. Please call AddClientMetadataParsing in ConfigureServices");
        }

        return app.UseMiddleware<ClientMetadataParsingMiddleware>();
    }
}
