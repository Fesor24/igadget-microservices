namespace OrderService.Models;

public class ShoppingCart
{
    public string Id { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
}
