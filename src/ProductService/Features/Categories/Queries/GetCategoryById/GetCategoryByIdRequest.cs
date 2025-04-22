using MediatR;
using ProductService.Response;

namespace ProductService.Features.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdRequest(Guid Id) :
    IRequest<GetCategoryResponse>;
