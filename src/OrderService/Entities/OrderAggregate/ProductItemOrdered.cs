namespace OrderService.Entities.OrderAggregate;

public class ProductItemOrdered
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string ImageUrl { get; set; }
}
