using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Data;

namespace ProductService.IntegrationTests.Utils;
public static class ServiceCollectionExtensions
{
    public static void RemoveDbContext<T>(this IServiceCollection services) where T : class
    {
        var dbContextDescriptor = services.SingleOrDefault(d =>
           d.ServiceType == typeof(DbContextOptions<ProductDbContext>));

        if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);
    }

    public static void EnsureCreated<T>(this IServiceCollection services) where T: class
    {
        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

        db.Database.Migrate();

        Database.InitializeDb(db);
    }
}
