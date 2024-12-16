
using UAParser;

namespace Turg.App.Middlewares.ClientMetadataParsing;

internal class ClientMetadataParsingMiddleware : IMiddleware
{
    private readonly Parser _parser;

    public ClientMetadataParsingMiddleware(Parser parser)
    {
        _parser = parser;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var userAgent = context.Request.Headers["User-Agent"];

        if (!string.IsNullOrWhiteSpace(userAgent))
        {
            var parsedUserAgent = _parser.Parse(userAgent);
            var clientMetadata = new ClientMetadata(parsedUserAgent);

            context.Items["ClientMetadata"] = clientMetadata;
        }

        await next(context);
    }
}
