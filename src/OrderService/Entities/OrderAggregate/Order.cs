using OrderService.Enums;

namespace OrderService.Entities.OrderAggregate;

public class Order
{
    public Guid Id { get; set; }
    public Address DeliveryAddress { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public decimal SubTotal { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public string BuyerEmail { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public decimal GetTotal() =>
        SubTotal + DeliveryMethod.Price;
}
