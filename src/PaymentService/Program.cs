using MassTransit;
using PaymentService.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(config =>
{
    config.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("payment",false));
    
    config.AddConsumersFromNamespaceContaining<OrderCreatedConsumer>();
    
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "", hst =>
        {
            hst.Username("guest");
            hst.Password("guest");
        });
        
        cfg.ConfigureEndpoints(ctx);
    });
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
