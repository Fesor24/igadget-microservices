using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;
using Shared.Contracts;

namespace SearchService.Consumers;

public class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        var result = await DB.DeleteAsync<Product>(context.Message.Id);

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(ProductDeleted),"Nothing was deleted from mongo collection");
    }
}
