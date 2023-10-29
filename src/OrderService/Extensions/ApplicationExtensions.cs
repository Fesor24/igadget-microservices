using Microsoft.EntityFrameworkCore;
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
    }
}
