using MediatR;
using ProductService.Response;

namespace ProductService.Features.Brands.Queries.GetBrandById;

public sealed record GetBrandByIdRequest(Guid Id) : IRequest<GetBrandResponse>;
