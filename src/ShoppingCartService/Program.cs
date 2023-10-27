using ShoppingCartService.EndpointDefinitions;
using ShoppingCartService.Repositories.Contracts;
using ShoppingCartService.Repositories.Implementation;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
{
    var configurationOptions = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")
        ,true);

    return ConnectionMultiplexer.Connect(configurationOptions);
});

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

var app = builder.Build();

ShoppingCartEndpointDefinition.RegisterEndpoint(app);

app.Run();
