using OrderService.Entities;
using OrderService.Models;

namespace OrderService.Services.Contracts;

public interface IGrpcClient
{
    ShoppingCart GetShoppingCart(string id);
    Product GetProduct(string id);
}
