using FluentValidation;
using MediatR;
using ProductService.Response;

namespace ProductService.Features.Products.Commands.Create;

public record CreateProductCommand : IRequest<GetProductResponse>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int YearOfRelease { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }  
}

public sealed class CraeteProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CraeteProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can not be empty")
            .NotNull().WithMessage("Name can not be null");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description can not be empty")
            .NotNull().WithMessage("Description can not be null");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Image url can not be empty")
            .NotNull().WithMessage("Image url can not be null");
    }
}
