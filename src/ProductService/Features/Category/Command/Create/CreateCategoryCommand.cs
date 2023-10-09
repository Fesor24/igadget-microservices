using MediatR;

namespace ProductService.Features.Category.Command.Create;

public class CreateCategoryCommand : IRequest<Guid>
{
    public string Name { get; set; }    
}
