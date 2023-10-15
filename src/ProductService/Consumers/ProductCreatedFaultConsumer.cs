using MassTransit;
using Shared.Contracts;

namespace ProductService.Consumers;

public class ProductCreatedFaultConsumer : IConsumer<Fault<ProductCreated>>
{
    private readonly ILogger<ProductCreatedFaultConsumer> _logger;

    public ProductCreatedFaultConsumer(ILogger<ProductCreatedFaultConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Fault<ProductCreated>> context)
    {
        var exceptions = context.Message.Exceptions;

        foreach (var exception in exceptions)
        {
            _logger.LogError($"An error occurred while consummation. " +
                $"Message: {exception.Message} \n Details: {exception.StackTrace}");
        }

        return Task.CompletedTask;
    }
}
