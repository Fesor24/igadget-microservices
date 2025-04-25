using OrderService.Entities.OrderAggregate;
using OrderService.Requests;
using OrderService.Response;

namespace OrderService.Services.Contracts;

public interface IOrderService
{
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest orderRequest);

    Task<IReadOnlyList<GetOrderResponse>> GetOrdersForUserAsync();

    Task<GetOrderResponse> GetOrderByIdAsync(Guid id);
    Task<bool> CancellationRequest(Guid orderId);
    Task CancelOrderAsync(Guid orderId);
}
