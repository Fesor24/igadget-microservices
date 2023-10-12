using FluentValidation;
using MediatR;

namespace ProductService.Features.Category.Command.Create;

public sealed class CreateCategoryCommand : IRequest<Guid>
{
    public string Name { get; set; }    
}

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can not be empty")
            .NotNull().WithMessage("Name can not be null");
    }
}
