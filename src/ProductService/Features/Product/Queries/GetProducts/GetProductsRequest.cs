using MediatR;
using ProductService.Response;

namespace ProductService.Features.Product.Queries.GetProducts;

public class GetProductsRequest : IRequest<IReadOnlyList<GetProductResponse>>
{
}
