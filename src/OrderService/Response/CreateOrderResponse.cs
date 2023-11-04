using OrderService.Enums;

namespace OrderService.Response;

public class CreateOrderResponse
{
    public string OrderId { get; set; }
    public AddressResponse DeliveryAddress { get; set; }
    public List<OrderItemsResponse> OrderItems { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Total { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTimeOffset OrderDate { get; set; }
}
