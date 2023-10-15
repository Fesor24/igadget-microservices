using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;
using Shared.Contracts;

namespace SearchService.Consumers;

public class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        await DB.DeleteAsync<Product>(context.Message.Id);
    }
}
