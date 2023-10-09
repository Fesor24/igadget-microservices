using MediatR;
using ProductService.Response;

namespace ProductService.Features.Category.Queries.GetCategoryById;

public class GetCategoryByIdRequest  :IRequest<GetCategoryResponse>
{
    public Guid Id { get; set; }
}
