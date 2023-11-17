using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using Shared.Contracts;

namespace OrderService.Consumers;

public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    private readonly OrderDbContext _dbContext;

    public ProductUpdatedConsumer(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        Guid productId;

        var validGuid = Guid.TryParse(context.Message.Id, out productId);

        if(!validGuid) return;

        var product = await _dbContext.Product
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product is null) return;

        product.Name = context.Message.Name;
        product.Description = context.Message.Description;
        product.ImageUrl = context.Message.ImageUrl;
        product.Price = context.Message.Price;

        _dbContext.Product.Update(product);

        await _dbContext.SaveChangesAsync();
    }
}
