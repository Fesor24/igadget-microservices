using FluentValidation;
using MediatR;

namespace ProductService.Features.Categories.Command.Create;

public sealed record CreateCategoryCommand(string Name) : IRequest<Guid>;

public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can not be empty")
            .NotNull().WithMessage("Name can not be null");
    }
}
