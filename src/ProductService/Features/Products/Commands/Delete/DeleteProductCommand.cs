using MediatR;

namespace ProductService.Features.Products.Commands.Delete;

public sealed record DeleteProductCommand(Guid Id) : IRequest<bool>;
