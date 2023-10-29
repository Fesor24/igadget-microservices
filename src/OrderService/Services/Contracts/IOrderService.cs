using OrderService.Requests;

namespace OrderService.Services.Contracts;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrderRequest orderRequest);
}
