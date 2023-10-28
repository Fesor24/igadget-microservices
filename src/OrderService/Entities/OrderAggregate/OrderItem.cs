namespace OrderService.Entities.OrderAggregate;

public class OrderItem
{
    public Guid Id { get; set; }
    public ProductItemOrdered ItemOrdered { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
