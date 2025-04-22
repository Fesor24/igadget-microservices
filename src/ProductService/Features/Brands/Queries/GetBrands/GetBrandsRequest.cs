using MediatR;
using ProductService.Response;

namespace ProductService.Features.Brands.Queries.GetBrands;

public sealed record GetBrandsRequest : IRequest<IReadOnlyList<GetBrandResponse>>;
