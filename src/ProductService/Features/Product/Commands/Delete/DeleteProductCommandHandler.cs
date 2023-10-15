using MediatR;
using ProductService.DataAccess.Contracts;
using Shared.Exceptions;

namespace ProductService.Features.Product.Commands.Delete;

public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(request.Id);

        if (product is null)
            throw new ApiNotFoundException($"Product with Id:{request.Id} not found");

        _unitOfWork.ProductRepository.DeleteAsync(product);

        var result = await _unitOfWork.Complete();

        if (result < 1)
            throw new ApiBadRequestException("No record was deleted from database");

        return true;
    }
}
