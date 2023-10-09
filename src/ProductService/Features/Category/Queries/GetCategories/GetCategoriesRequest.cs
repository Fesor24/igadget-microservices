using MediatR;
using ProductService.Response;

namespace ProductService.Features.Category.Queries.GetCategories;

public class GetCategoriesRequest : IRequest<IReadOnlyList<GetCategoryResponse>>
{
}
