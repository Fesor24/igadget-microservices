using OrderService.Requests;
using OrderService.Services.Contracts;

namespace OrderService.Services.Implementation;

public class OrderService : IOrderService
{
    public Task CreateOrderAsync(CreateOrderRequest orderRequest)
    {
        throw new NotImplementedException();
    }
}
