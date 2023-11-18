using OrderService.Entities.OrderAggregate;

namespace OrderService.DataAccess.Specifications.Delivery;

public class GetDeliverySpecification : BaseSpecification<DeliveryMethod>
{
    public GetDeliverySpecification(Guid deliveryId) :  base(x => x.Id == deliveryId)
    {
        
    }

    public GetDeliverySpecification()
    {
        
    }
}
