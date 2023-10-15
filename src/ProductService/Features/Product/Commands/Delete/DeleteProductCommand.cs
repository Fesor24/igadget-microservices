using MediatR;

namespace ProductService.Features.Product.Commands.Delete;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;
