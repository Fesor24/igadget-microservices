using OrderService.Entities.OrderAggregate;
using OrderService.Requests;

namespace OrderService.Services.Contracts;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(CreateOrderRequest orderRequest);

    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

    Task<Order> GetOrderByIdAsync(string id);
}
