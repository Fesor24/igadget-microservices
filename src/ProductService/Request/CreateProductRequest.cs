using FluentValidation;
using ProductService.Helper;

namespace ProductService.Request;

public class CreateProductRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int YearOfRelease { get; set; }
    public string ImageUrl { get; set; }
    public string CategoryId { get; set; }
    public string BrandId { get; set; }
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
            .WithMessage("Name can not be null or empty");

        RuleFor(x => x.Id)
            .Must(CustomValidators.IsValidGuid)
            .WithMessage("Pass in a valid Guid -- Product Id");

        RuleFor(x => x.BrandId)
            .Must(CustomValidators.IsValidGuid)
            .WithMessage("Pass in a valid Guid -- Brand Id");

        RuleFor(x => x.CategoryId)
            .Must(CustomValidators.IsValidGuid)
            .WithMessage("Pass in a valid Guid -- Category id");

    }
}
