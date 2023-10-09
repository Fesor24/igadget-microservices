using ProductService.Definitions.Contracts;

namespace ProductService.Extensions;

public static class EndpointExtension
{
    public static void RegisterEndpoints(this WebApplication app)
    {
        var endpointDefinitions = typeof(Program).Assembly.GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IEndpointDefinition)) && !x.IsAbstract && !x.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointDefinition>();

        foreach(var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.RegisterEndpoints(app);
        }
    }
}
