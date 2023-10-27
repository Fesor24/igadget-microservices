using ShoppingCartService.Entities;
using ShoppingCartService.Repositories.Contracts;

namespace ShoppingCartService.EndpointDefinitions;

public class ShoppingCartEndpointDefinition
{
    public static void RegisterEndpoint(WebApplication app)
    {
        var cart = app.MapGroup("/api/Cart");

        cart.MapGet("/{cartId}", GetCart);

        cart.MapPost("/", AddEditCart);

        cart.MapDelete("/{cartId}", DeleteCart);
    }

    private static async Task<IResult> GetCart(IShoppingCartRepository cartRepository, string cartId)
    {
        var cart = await cartRepository.GetCartAsync(cartId);

        return Results.Ok(cart ?? new ShoppingCart(cartId));
        
    }

    private static async Task<IResult> AddEditCart(IShoppingCartRepository cartRepository, ShoppingCart cart)
    {
        var shoppingCart = await cartRepository.AddEditCartAsync(cart);

        return Results.Ok(shoppingCart);
    }
    
    private static async Task<IResult> DeleteCart(IShoppingCartRepository cartRepository, string cartId)
    {
        await cartRepository.DeleteCartAsync(cartId);

        return Results.NoContent();
    }
}
