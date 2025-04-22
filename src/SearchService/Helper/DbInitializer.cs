using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.Services;

namespace SearchService.Helper;

public static class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("productdb", MongoClientSettings.FromConnectionString(
            app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Product>()
            .Key(x => x.Name, KeyType.Text)
            .Key(x => x.Brand, KeyType.Text)
            .Key(x => x.Category, KeyType.Text)
            .CreateAsync();

        var scope = app.Services.CreateScope();

        var prdService = scope.ServiceProvider.GetRequiredService<ProductHttpService>();

        var dbProducts = await DB.Find<Product>().ExecuteAsync();

        if (dbProducts.Any())
            return;

        var products = await prdService.GetProducts();

        if (products is not null && products.Any())
        {
            Console.WriteLine($"{products.Count} returned from api");
            await products.SaveAsync();
        }
    }
}
