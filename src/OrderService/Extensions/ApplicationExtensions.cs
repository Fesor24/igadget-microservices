using Microsoft.EntityFrameworkCore;
using OrderService.Data;

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
    }
}
