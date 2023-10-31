namespace OrderService.Requests;

public class CreateOrderRequest
{
    public string BuyerEmail { get; set; }
    public string DeliveryMethodId { get; set; }
    public AddressRequest DeliveryAddress { get; set; }
    public string CartId { get; set; }
}
