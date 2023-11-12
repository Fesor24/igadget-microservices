using MediatR;
using ProductService.DataAccess.Contracts;
using Shared.Exceptions;

namespace ProductService.Features.Product.Commands.Update;

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(request.Id);

        if (product is null)
            throw new ApiNotFoundException($"Product with Id: {request.Id} not found");

        product.Name = string.IsNullOrWhiteSpace(request.Name) ? product.Name : request.Name;
        product.ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? product.ImageUrl : request.ImageUrl;
        product.Description = string.IsNullOrWhiteSpace(request.Description) ? product.Description : request.Description;

        _unitOfWork.ProductRepository.Update(product);

        await _unitOfWork.Complete();

        return Unit.Value;
    }
}
