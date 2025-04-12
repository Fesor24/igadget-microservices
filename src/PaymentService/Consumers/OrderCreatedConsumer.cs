using MassTransit;
using Shared.Contracts;

namespace PaymentService.Consumers;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        Console.WriteLine($"Payment processing for order with Id: {context.Message.OrderId}");
        return Task.CompletedTask;
    }
}