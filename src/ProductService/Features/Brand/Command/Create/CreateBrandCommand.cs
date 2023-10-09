using MediatR;

namespace ProductService.Features.Brand.Command.Create;

public class CreateBrandCommand : IRequest<Guid>
{
    public string Name { get; set; }
}
