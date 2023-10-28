namespace OrderService.Entities.OrderAggregate;

public class DeliveryMethod
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DeliveryTime { get; set; }
    public decimal Price { get; set; }
}
