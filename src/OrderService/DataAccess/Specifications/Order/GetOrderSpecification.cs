using OrderEntity = OrderService.Entities.OrderAggregate.Order;

namespace OrderService.DataAccess.Specifications.Order;

public class GetOrderSpecification : BaseSpecification<OrderEntity>
{
    public GetOrderSpecification(Guid orderId) : base(x => x.Id == orderId)
    {
        AddIncludes(x => x.DeliveryMethod);
        AddIncludes(x => x.OrderItems);
    }

    public GetOrderSpecification(string email) : base(x => x.BuyerEmail == email)
    {
        AddIncludes(x => x.DeliveryMethod);
        AddIncludes(x => x.OrderItems);
        AddOrderByDescending(x => x.OrderDate);
    }
}
