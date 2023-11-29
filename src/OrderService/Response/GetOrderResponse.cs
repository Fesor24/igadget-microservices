namespace OrderService.Response;

public class GetOrderResponse
{
    public string OrderId { get; set; }
    public List<OrderItemsResponse> OrderItems { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentStatus { get; set; }
    public decimal SubTotal { get; set; }
    public decimal Total { get; set; }  
    public decimal DeliveryCharges { get; set; }
    public string Date { get; set; }
}
