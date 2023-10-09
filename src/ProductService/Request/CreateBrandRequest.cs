using FluentValidation;
using ProductService.Helper;

namespace ProductService.Request;

public class CreateBrandRequest
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class CreateBrandRequestValidator : AbstractValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name can not be null or empty");

        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull()
            .WithMessage("Id can not be null or empty");

        RuleFor(x => x.Id)
            .Must(CustomValidators.IsValidGuid)
            .WithMessage("Value(Brand Id) must be a valid Guid");
    }
}
