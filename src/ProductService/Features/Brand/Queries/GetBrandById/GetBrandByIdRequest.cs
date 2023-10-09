using MediatR;
using ProductService.Response;

namespace ProductService.Features.Brand.Queries.GetBrandById;

public class GetBrandByIdRequest : IRequest<GetBrandResponse>
{
    public Guid Id { get; set; }
}
