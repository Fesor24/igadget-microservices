namespace OrderService.Requests;

public class OrderItemsRequest
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
