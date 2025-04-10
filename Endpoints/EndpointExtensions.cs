using System.Reflection;

namespace Turg.App.Endpoints;

internal static class EndpointExtensions
{
    /// <summary>
    /// Discovers <see cref="IEndpoints"/> from the given <see cref="Assembly"/> and adds into given <see cref="IServiceCollection">.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="executingAssembly"></param>
    internal static void AddEndpoints(this IServiceCollection services, Assembly executingAssembly)
    {
        var endpointTypes = executingAssembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && type.GetInterfaces().Contains(typeof(IEndpoints)));
        
        foreach (var endpointType in endpointTypes)
        {
            var descriptor = ServiceDescriptor.Singleton(typeof(IEndpoints), endpointType);
            services.Add(descriptor);
        }
    }

    /// <summary>
    /// Resolves and maps <see cref="IEndpoints"/> to <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="routeBuilder"></param>
    /// <param name="services"></param>
    internal static void MapEndpoints(this IEndpointRouteBuilder routeBuilder, IServiceProvider services)
    {
        var endpoints = services.GetServices<IEndpoints>();

        foreach (var endpoint in endpoints)
        {
            endpoint.Map(routeBuilder);
        }
    }
}
