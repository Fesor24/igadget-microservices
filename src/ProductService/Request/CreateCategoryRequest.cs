using FluentValidation;
using ProductService.Helper;

namespace ProductService.Request;

public class CreateCategoryRequest
{
    public string Name { get; set; }
}

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name can not be null or empty");
    }
}
