using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DataAccess.Contracts;
using ProductService.DataAccess.Repository;

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

        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        
    }
}
