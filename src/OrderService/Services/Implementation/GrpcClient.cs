using Grpc.Net.Client;
using OrderService.Entities;
using OrderService.Models;
using OrderService.Services.Contracts;
using ShoppingCartService;
using System.Text.Json;

namespace OrderService.Services.Implementation;

public class GrpcClient : IGrpcClient
{
    private readonly IConfiguration _config;
    private readonly ILogger<GrpcClient> _logger;

    public GrpcClient(IConfiguration config, ILogger<GrpcClient> logger)
    {
        _config = config;
        _logger = logger;
    }

    public Product GetProduct(string id)
    {
        throw new NotImplementedException();
    }

    public ShoppingCart GetShoppingCart(string id)
    {
        var channel = GrpcChannel.ForAddress(_config["GrpcCart"]);

        var client = new GrpcCart.GrpcCartClient(channel);

        var request = new GetCartRequest { Id = id };

        try
        {
            var response = client.GetCart(request);

            _logger.LogInformation("Grpc Cart Response: {response}", JsonSerializer.Serialize(response));

            List<ShoppingCartItem> cartItems = new();

            foreach(var item in response.Cart.CartItems)
            {
                var cartItem = new ShoppingCartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductName = item.ProductName,
                    ImageUrl = item.ImageUrl
                };

                cartItems.Add(cartItem);
            }

            var shoppingCart = new ShoppingCart
            {
                Id = response.Cart.Id,
                Items = cartItems
            };

            return shoppingCart;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while sending a request to grpc server. Message: {ex.Message}" +
                $"Details: {ex.StackTrace}");

            return null;
        }
    }
}
