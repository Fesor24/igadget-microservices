namespace OrderService.Response;

public class GetDeliveryMethodResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DeliveryTime { get; set; }
    public decimal Price { get; set; }
}
