using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using Shared.Contracts;

namespace OrderService.Consumers;

public class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    private readonly OrderDbContext _context;

    public ProductDeletedConsumer(OrderDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        var product = await _context.Product
            .FirstOrDefaultAsync(x => x.Id == Guid.Parse(context.Message.Id));

        if (product is null)
            return;

        _context.Product.Remove(product);

        await _context.SaveChangesAsync();
    }
}
