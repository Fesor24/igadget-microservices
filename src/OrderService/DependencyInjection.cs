using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderService.Consumers;
using OrderService.Data;
using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Repository;
using OrderService.Services.Contracts;
using OrderService.Services.Implementation;
using OrdSvc = OrderService.Services.Implementation.OrderService;

namespace OrderService;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        AddPersistence(services, config);
        
        AddAuthentication(services, config);

        services.AddScoped<IOrderService, OrdSvc>();

        services.AddScoped<IDeliveryMethodService, DeliveryMethodService>();

        services.AddHttpContextAccessor();

        AddMassTransit(services, config);

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

    private static void AddMassTransit(IServiceCollection services, IConfiguration config)
    {
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
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<OrderDbContext>(options =>
        {
            options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });
    }
    
    public static void AddAuthentication(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                //opt.Authority = config["IdentityServiceUrl"];
                opt.Authority = config["Authentication:Issuer"];
                opt.Audience = config["Authentication:Audience"];
                //opt.Audience = "orderapi";
                opt.RequireHttpsMetadata = false;

                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = config["Authentication:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = config["Authentication:Issuer"],
                    ValidTypes = ["JWT"],
                };
            });

        services.AddAuthorization();
    }

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        OrderDbContext context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        context.Database.Migrate();
    }
}
