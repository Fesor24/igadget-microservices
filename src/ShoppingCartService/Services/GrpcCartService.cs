using Google.Protobuf.Collections;
using Grpc.Core;
using ShoppingCartService.Repositories.Contracts;

namespace ShoppingCartService.Services;

public class GrpcCartService : GrpcCart.GrpcCartBase
{
    private readonly IShoppingCartRepository _shoppingCart;

    public GrpcCartService(IShoppingCartRepository shoppingCart)
    {
        _shoppingCart = shoppingCart;
    }

    public override async Task<GrpcCartResponse> GetCart(GetCartRequest request, ServerCallContext context)
    {
        var cart = await _shoppingCart.GetCartAsync(request.Id) ?? 
            throw new RpcException(new Status(StatusCode.NotFound, $"Cart with Id: {request.Id} not found"));

        var cartItems = new RepeatedField<GrpcCartItemsModel>();

        foreach(var item in cart.Items)
        {
            var cartItem = new GrpcCartItemsModel
            {
                ImageUrl = item.ImageUrl,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                ProductName = item.ProductName
            };

            cartItems.Add(cartItem);
        }

        var response = new GrpcCartResponse
        {
            Cart = new GrpcCartModel
            {
                Id = cart.Id
            }
        };

        response.Cart.CartItems.Add(cartItems);

        return response;
    }

    public override async Task<GrpcDeleteCartResponse> DeleteCart(GetCartRequest request, ServerCallContext context) =>
        new GrpcDeleteCartResponse
        {
            CartDeleted = await _shoppingCart.DeleteCartAsync(request.Id)
        };
        
}
