using MediatR;
using ProductService.DataAccess.Contracts;
using ProductEntity = ProductService.Entities.Product;

namespace ProductService.Features.Product.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ProductEntity product = new()
        {
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            YearOfRelease = request.YearOfRelease,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            Id = request.Id,
            BrandId = request.BrandId
        };

        await _unitOfWork.ProductRepository.AddAsync(product);

        await _unitOfWork.Complete();

        return product.Id;
    }
}
