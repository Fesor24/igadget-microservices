using ShoppingCartService.Entities;

namespace ShoppingCartService.Repositories.Contracts;

public interface IShoppingCartRepository
{
    Task<ShoppingCart> GetCartAsync(string cartId);

    Task<ShoppingCart> AddEditCartAsync(ShoppingCart cart);

    Task<bool> DeleteCartAsync(string cartId);
}
