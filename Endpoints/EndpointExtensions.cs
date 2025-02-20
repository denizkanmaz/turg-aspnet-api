using System.Reflection;

namespace Turg.App.Endpoints;

internal static class EndpointExtensions
{
    /// <summary>
    /// Discovers <see cref="IEndpoints"/> from the given <see cref="Assembly"/> and maps to the given <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="routeBuilder"></param>
    /// <param name="executingAssembly"></param>
    internal static void MapEndpoints(this IEndpointRouteBuilder routeBuilder, Assembly executingAssembly)
    {
        var types = executingAssembly.GetTypes();
        var endpointTypes = types.Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(typeof(IEndpoints)));
        var endpoints = endpointTypes.Select(type => (IEndpoints)Activator.CreateInstance(type));

        foreach (var endpoint in endpoints)
        {
            endpoint.Map(routeBuilder);
        }
    }
}
