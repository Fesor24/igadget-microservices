namespace ShoppingCartService.Entities;

public class ShoppingCart
{
    public ShoppingCart()
    {
        
    }

    public ShoppingCart(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
    public List<ShoppingCartItems> Items { get; set; } = new();
}
