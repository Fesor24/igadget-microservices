using MassTransit;
using Shared.Contracts;

namespace OrderService.Consumers;

public sealed class OrderCancelledConsumer : IConsumer<OrderCancelled>
{
    public Task Consume(ConsumeContext<OrderCancelled> context)
    {
        Console.WriteLine("Order cancelled...");
        Console.WriteLine(context.Message.OrderId + " is the order id");
        return Task.CompletedTask;
    }
}