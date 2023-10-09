using MediatR;
using ProductService.Response;

namespace ProductService.Features.Brand.Queries.GetBrands;

public class GetBrandsRequest : IRequest<IReadOnlyList<GetBrandResponse>>
{
}
