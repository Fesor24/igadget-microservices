using OrderService.Requests;
using OrderService.Services.Contracts;

namespace OrderService.Endpoints;

public class DeliveryEndpointDefinition : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var delivery = app.MapGroup("/api/delivery");

        delivery.MapPost("/", CreateDeliveryMethod);

        delivery.MapGet("/", GetDeliveryMethods);

        delivery.MapGet("/{id}", GetDeliveryMethod);
    }

    private static async Task<IResult> CreateDeliveryMethod(CreateDeliveryMethodRequest request, IDeliveryMethodService
        deliveryService) => 
       Results.Ok(await deliveryService.CreateDeliveryMethod(request));

    private static async Task<IResult> GetDeliveryMethod(Guid id, IDeliveryMethodService deliveryService) =>
        Results.Ok(await deliveryService.GetDeliveryMethod(id));

    private static async Task<IResult> GetDeliveryMethods(IDeliveryMethodService deliveryService) =>
        Results.Ok(await deliveryService.GetDeliveryMethods());
}
