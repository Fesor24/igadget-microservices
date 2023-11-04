using OrderService.Endpoints;
using System.Reflection;

namespace OrderService.Extensions;

public static class EndpointExtensions
{
    public static void RegisterApplicationEndpoints(this WebApplication app)
    {
        var endpoints = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => typeof(IEndpointDefinition).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach(var endpoint in endpoints)
        {
            endpoint.RegisterEndpoints(app);
        }

    }
}
