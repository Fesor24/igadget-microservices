using MediatR;

namespace ProductService.Features.Brand.Command.Create;

public class CreateBrandCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
