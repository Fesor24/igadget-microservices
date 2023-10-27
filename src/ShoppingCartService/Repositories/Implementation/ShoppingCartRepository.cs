using ShoppingCartService.Entities;
using ShoppingCartService.Repositories.Contracts;
using StackExchange.Redis;
using System.Text.Json;

namespace ShoppingCartService.Repositories.Implementation;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly IDatabase _db;

    public ShoppingCartRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _db = connectionMultiplexer.GetDatabase();
    }

    public async Task<ShoppingCart> AddEditCartAsync(ShoppingCart cart)
    {
        var serCart = JsonSerializer.Serialize(cart);

        var result = await _db.StringSetAsync(cart.Id, serCart, expiry: TimeSpan.FromDays(30));

        return result ? await GetCartAsync(cart.Id) : null; 
    }

    public async Task<bool> DeleteCartAsync(string cartId) => 
        await _db.KeyDeleteAsync(cartId);

    public async Task<ShoppingCart> GetCartAsync(string cartId)
    {
        var cart = await _db.StringGetAsync(cartId);

        return cart.IsNullOrEmpty ? null : 
            JsonSerializer.Deserialize<ShoppingCart>(cart);
    }
}
