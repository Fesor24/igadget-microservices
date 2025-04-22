using MediatR;
using ProductService.Response;

namespace ProductService.Features.Products.Queries.GetProducts;

public sealed record GetProductsRequest : 
    IRequest<IReadOnlyList<GetProductResponse>>;
