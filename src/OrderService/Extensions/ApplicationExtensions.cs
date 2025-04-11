using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Consumers;
using OrderService.Data;
using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Repository;
using OrderService.Services.Contracts;
using OrderService.Services.Implementation;
using OrdSvc = OrderService.Services.Implementation.OrderService;

namespace OrderService.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IOrderService, OrdSvc>();

        services.AddScoped<IDeliveryMethodService, DeliveryMethodService>();

        services.AddHttpContextAccessor();

        services.AddMassTransit(options =>
        {
            options.AddConsumersFromNamespaceContaining<ProductCreatedConsumer>();

            options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("order", false));

            options.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config.GetValue("RabbitMq:Username", "guest"));
                    host.Password(config.GetValue("RabbitMq:Password", "guest"));
                });

                cfg.ReceiveEndpoint("order-product-created", opt =>
                {
                    opt.UseMessageRetry(conf => conf.Interval(6, 6));

                    opt.ConfigureConsumer<ProductCreatedConsumer>(ctx);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        services.AddScoped<IGrpcClient, GrpcClient>();

        services.AddHttpContextAccessor();

        services.AddCors(policy =>
        {
            policy.AddPolicy("CorsPolicy", pol =>
            {
                pol.AllowAnyMethod().AllowAnyHeader().AllowAnyMethod();
            });
        });
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        OrderDbContext context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        context.Database.Migrate();
    }
}
