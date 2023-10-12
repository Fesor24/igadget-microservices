using FluentValidation;
using MediatR;
using ProductService.Features.Product.Commands.Create;

namespace ProductService.Features.Brand.Command.Create;

public sealed class CreateBrandCommand : IRequest<Guid>
{
    public string Name { get; set; }
}

public sealed class CreateBrandCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name)
           .NotNull().WithMessage("Name can not be null")
           .NotEmpty().WithMessage("Name can not be empty");
    }
}
