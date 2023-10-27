namespace ShoppingCartService.Entities;

public class ShoppingCartItems
{
    public string Id { get; set; }
    public string Product { get; set; } 
    public decimal Price { get; set; }  
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
}
