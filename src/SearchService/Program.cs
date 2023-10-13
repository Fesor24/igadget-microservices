using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

await DB.InitAsync("Search", MongoClientSettings.FromConnectionString(
    app.Configuration.GetConnectionString("MongoDbConnection")));

await DB.Index<Product>()
    .Key(x => x.Name, KeyType.Text)
    .Key(x => x.BrandName, KeyType.Text)
    .Key(x => x.CategoryName, KeyType.Text)
    .CreateAsync();

app.Run();
