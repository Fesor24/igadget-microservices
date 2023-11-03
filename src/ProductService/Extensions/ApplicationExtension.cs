using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Behaviors;
using ProductService.Consumers;
using ProductService.Data;
using ProductService.DataAccess.Contracts;
using ProductService.DataAccess.Repository;
using System.Reflection;

namespace ProductService.Extensions;

public static class ApplicationExtension
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ProductDbContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddMassTransit(opt =>
        {
            opt.AddEntityFrameworkOutbox<ProductDbContext>(ef =>
            {
                ef.QueryDelay = TimeSpan.FromMinutes(5); // Query will run every 5 mins...to check for pending messages in the db

                ef.UsePostgres();

                ef.UseBusOutbox();
            });

            opt.AddConsumersFromNamespaceContaining<ProductCreatedFaultConsumer>();

            opt.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("product", false));

            opt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config["RabbitMq:Host"], "/", host =>
                {
                    host.Username(config.GetValue("RabbitMq:Username", "guest"));
                    host.Password(config.GetValue("RabbitMq:Password", "guest"));
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddGrpc();
        
    }
}
