using MassTransit;
using Shared.Contracts;

namespace OrderService.Consumers;

public sealed class OrderCancelledConsumer(IPublishEndpoint publishEndpoint) : IConsumer<OrderCancelled>
{
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    public async Task Consume(ConsumeContext<OrderCancelled> context)
    {
        Console.WriteLine("Order cancelled...");
        Console.WriteLine(context.Message.OrderId + " is the order id");
        // logic to handle cancellation...
        
        // if cancellation successful, raise order completion event so the state instance is finalized and removed...
        OrderCompleted orderCompleted = new(context.Message.OrderId);
        await _publishEndpoint.Publish(orderCompleted);
    }
}