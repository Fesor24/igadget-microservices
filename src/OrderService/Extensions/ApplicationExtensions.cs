using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Consumers;
using OrderService.Data;
using OrderService.Services.Contracts;
using OrdSvc = OrderService.Services.Implementation.OrderService;

namespace OrderService.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IOrderService, OrdSvc>();

        services.AddHttpContextAccessor();

        services.AddMassTransit(options =>
        {
            options.AddConsumersFromNamespaceContaining<ProductCreatedConsumer>();

            options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("order", false));

            options.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(config["RabbitMq:Host"], host =>
                {
                    host.Username(config.GetValue(config["RabbitMq:Username"], "guest"));
                    host.Password(config.GetValue(config["RabbitMq:Password"], "guest"));
                });

                cfg.ReceiveEndpoint("order-product-created", opt =>
                {
                    opt.UseMessageRetry(conf => conf.Interval(6, 6));

                    opt.ConfigureConsumer<ProductCreatedConsumer>(ctx);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });
    }
}
