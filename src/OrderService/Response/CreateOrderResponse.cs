namespace OrderService.Response;

public class CreateOrderResponse
{
    public string OrderId { get; set; }
    public AddressResponse DeliveryAddress { get; set; }
    public List<OrderItemsResponse> OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentStatus { get; set; }
    public string Date { get; set; }
}
