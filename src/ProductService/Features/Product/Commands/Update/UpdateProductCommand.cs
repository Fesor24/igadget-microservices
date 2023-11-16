using MediatR;

namespace ProductService.Features.Product.Commands.Update;

public class UpdateProductCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
}
