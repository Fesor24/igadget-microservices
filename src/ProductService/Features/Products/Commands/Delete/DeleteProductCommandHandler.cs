using MassTransit;
using MediatR;
using ProductService.DataAccess.Contracts;
using Shared.Contracts;
using Shared.Exceptions;

namespace ProductService.Features.Products.Commands.Delete;

internal sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(request.Id);

        if (product is null)
            throw new ApiNotFoundException($"Product with Id:{request.Id} not found");

        _unitOfWork.ProductRepository.DeleteAsync(product);

        await _publishEndpoint.Publish(new ProductDeleted { Id = request.Id.ToString() });

        var result = await _unitOfWork.Complete();

        if (result < 1)
            throw new ApiBadRequestException("No record was deleted from database");

        return true;
    }
}
