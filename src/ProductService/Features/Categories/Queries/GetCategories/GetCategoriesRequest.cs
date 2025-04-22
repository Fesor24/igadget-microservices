using MediatR;
using ProductService.Response;

namespace ProductService.Features.Categories.Queries.GetCategories;

public sealed record GetCategoriesRequest : IRequest<IReadOnlyList<GetCategoryResponse>>;
