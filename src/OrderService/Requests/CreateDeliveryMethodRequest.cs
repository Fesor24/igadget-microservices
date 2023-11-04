namespace OrderService.Requests;

public class CreateDeliveryMethodRequest
{
    public string Name { get; set; }
    public string DeliveryTime { get; set; }
    public decimal Price { get; set; }
}
