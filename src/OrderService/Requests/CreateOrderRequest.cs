namespace OrderService.Requests;

public class CreateOrderRequest
{
    public string DeliveryMethodId { get; set; }
    public AddressRequest DeliveryAddress { get; set; }
    public string CartId { get; set; }
}
