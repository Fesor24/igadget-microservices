using OrderService.Requests;
using OrderService.Response;

namespace OrderService.Services.Contracts;

public interface IDeliveryMethodService
{
    Task<GetDeliveryMethodResponse> CreateDeliveryMethod(CreateDeliveryMethodRequest request);
    Task<IReadOnlyList<GetDeliveryMethodResponse>> GetDeliveryMethods();
    Task<GetDeliveryMethodResponse> GetDeliveryMethod(Guid  deliveryMethodId);
}
