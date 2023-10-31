using MassTransit;
using OrderService.Data;
using OrderService.Entities;
using Shared.Contracts;

namespace OrderService.Consumers;

public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly OrderDbContext _dbContext;

    public ProductCreatedConsumer(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        var product = new Product
        {
            Id = Guid.Parse(context.Message.Id),
            Name = context.Message.Name,
            ImageUrl = context.Message.ImageUrl,
            Brand = context.Message.Brand,
            Category = context.Message.Category,
            Price = context.Message.Price,
            YearOfRelease = context.Message.YearOfRelease,
            Description = context.Message.Description
        };

        await _dbContext.Product.AddAsync(product);

        await _dbContext.SaveChangesAsync();
    }
}
