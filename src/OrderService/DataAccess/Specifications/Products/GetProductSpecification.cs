using OrderService.Entities;

namespace OrderService.DataAccess.Specifications.Products;

public class GetProductSpecification : BaseSpecification<Product>
{
    public GetProductSpecification(Guid id) : base(x => x.Id == id)
    {
        
    }
}
