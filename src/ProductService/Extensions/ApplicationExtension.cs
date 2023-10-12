using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Behaviors;
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
        
    }
}
