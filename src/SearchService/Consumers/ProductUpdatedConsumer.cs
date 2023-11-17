using MassTransit;
using MongoDB.Entities;
using SearchService.Entities;
using Shared.Contracts;

namespace SearchService.Consumers;

public class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        await DB.Update<Product>()
            .MatchID(context.Message.Id)
            .Modify(x => x.Name, context.Message.Name)
            .Modify(x => x.Description, context.Message.Description)
            .Modify(x => x.ImageUrl, context.Message.ImageUrl)
            .Modify(x => x.Price, context.Message.Price)
            .ExecuteAsync();
    }
}
