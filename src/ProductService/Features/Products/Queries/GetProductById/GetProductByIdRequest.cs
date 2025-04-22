using MediatR;
using ProductService.Response;

namespace ProductService.Features.Products.Queries.GetProductById;

public sealed record GetProductByIdRequest(Guid Id) :
    IRequest<GetProductResponse>;
