using FluentValidation;
using MediatR;

namespace ProductService.Features.Product.Commands.Create;

public sealed class CreateProductCommand : IRequest<Guid>
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
            .NotEmpty().WithMessage("Desctiption can not be empty")
            .NotNull().WithMessage("Description can not be null");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Image url can not be empty")
            .NotNull().WithMessage("Image url can not be null");
    }
}
