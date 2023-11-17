using MassTransit;
using MediatR;
using ProductService.DataAccess.Contracts;
using Shared.Contracts;
using Shared.Exceptions;

namespace ProductService.Features.Product.Commands.Update;

internal sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publisher;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publisher)
    {
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAsync(request.Id)
            ?? throw new ApiNotFoundException($"Product with Id: {request.Id} not found");

        product.Name = string.IsNullOrWhiteSpace(request.Name) ? product.Name : request.Name;
        product.ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? product.ImageUrl : request.ImageUrl;
        product.Description = string.IsNullOrWhiteSpace(request.Description) ? product.Description : request.Description;
        product.Price = request.Price > 0 ? request.Price : product.Price;

        _unitOfWork.ProductRepository.Update(product);

        var res = await _unitOfWork.Complete();

        if(res > 0)
        {
            ProductUpdated updatedProduct = new()
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };

            await _publisher.Publish(updatedProduct);
        }

        return Unit.Value;
    }
}
