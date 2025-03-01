
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Turg.App.Filters;

internal class ValidationEndpointFilter<T> : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var input = context.Arguments.OfType<T>().FirstOrDefault();
        var validationContext = new ValidationContext(input);
        List<ValidationResult> validationResults = new();

        var isValid = Validator.TryValidateObject(input, validationContext, validationResults, true);

        if (!isValid)
        {
            var validationErrors = validationResults
                .GroupBy(result => result.MemberNames.FirstOrDefault() ?? "Other")
                .ToDictionary(group => group.Key, group => group.Select(error => error.ErrorMessage).ToArray());

            return TypedResults.ValidationProblem(validationErrors, extensions: new Dictionary<string, object>{{
                    "traceId", Activity.Current?.Id ?? context.HttpContext.TraceIdentifier
                }});
        }

        return await next(context);
    }
}

internal static class ValidationEndpointFilterExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ValidationEndpointFilter<T>>();
    }
}
