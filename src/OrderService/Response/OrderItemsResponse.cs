namespace OrderService.Response;

public class OrderItemsResponse
{
    public string OrderItemId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
