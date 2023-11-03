using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Entities.OrderAggregate;
using OrderService.Requests;
using OrderService.Services.Contracts;

namespace OrderService.Services.Implementation;

public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;
    private readonly IGrpcClient _grpcClient;

    public OrderService(OrderDbContext context, IGrpcClient grpcClient)
    {
        _context = context;
        _grpcClient = grpcClient;
    }

    public Task<Order> CreateOrderAsync(CreateOrderRequest orderRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetOrderByIdAsync(string id) =>
        await _context.Order
        .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail) =>
        await _context.Order
        .Where(x => x.BuyerEmail == buyerEmail)
        .ToListAsync();
}
