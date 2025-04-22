using FluentValidation;
using MediatR;
using ProductService.Features.Products.Commands.Create;

namespace ProductService.Features.Brands.Command.Create;

public sealed record CreateBrandCommand(string Name) : IRequest<Guid>;

public sealed class CreateBrandCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotNull().WithMessage("Name can not be null")
           .NotEmpty().WithMessage("Name can not be empty");
    }
}
