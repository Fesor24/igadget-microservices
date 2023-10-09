using FluentValidation;

namespace ProductService.Request;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int YearOfRelease { get; set; }
    public string ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }
}

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name can not be null or empty");

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .WithMessage("Description can not be null or empty");

        RuleFor(x => x.ImageUrl)
            .NotEmpty()
            .NotNull()
            .WithMessage("Image url can not be null or empty");
    }
}
