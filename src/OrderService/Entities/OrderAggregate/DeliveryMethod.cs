namespace OrderService.Entities.OrderAggregate;

public sealed class DeliveryMethod
{
    private DeliveryMethod()
    {
        
    }

    public DeliveryMethod(Guid id, string name, string deliveryTime, decimal price)
    {
        Id = id;
        Name = name;
        DeliveryTime = deliveryTime;
        Price = price;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string DeliveryTime { get; private set; }
    public decimal Price { get; private set; }
}
