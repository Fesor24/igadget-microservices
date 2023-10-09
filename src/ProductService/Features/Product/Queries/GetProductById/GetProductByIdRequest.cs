using MediatR;
using ProductService.Response;

namespace ProductService.Features.Product.Queries.GetProductById;

public class GetProductByIdRequest : IRequest<GetProductResponse>
{
    public Guid Id { get; set; }
}
