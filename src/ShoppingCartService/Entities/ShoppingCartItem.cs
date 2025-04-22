namespace ShoppingCartService.Entities;

public sealed class ShoppingCartItem
{
    public string ProductId { get; set; }
    public string ProductName { get; set; } 
    public decimal Price { get; set; }  
    public int Quantity { get; set; }
    public string ImageUrl { get; set; }
    public string Brand { get; set; }
    public string Category { get; set; }
}
