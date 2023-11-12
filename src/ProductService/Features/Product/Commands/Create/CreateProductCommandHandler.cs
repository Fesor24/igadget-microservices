using AutoMapper;
using MassTransit;
using MediatR;
using ProductService.DataAccess.Contracts;
using ProductService.Response;
using Shared.Contracts;
using Shared.Exceptions;
using ProductEntity = ProductService.Entities.Product;

namespace ProductService.Features.Product.Commands.Create;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, GetProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task<GetProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        ProductEntity product = new()
        {
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            YearOfRelease = request.YearOfRelease,
            ImageUrl = request.ImageUrl,
            Price = request.Price,
            Id = Guid.NewGuid(),
            BrandId = request.BrandId
        };

        await _unitOfWork.ProductRepository.AddAsync(product);

        var brand = await _unitOfWork.BrandRepository.GetAsync(request.BrandId);

        var category = await _unitOfWork.CategoryRepository.GetAsync(request.CategoryId);

        GetProductResponse productResponse = new()
        {
            Name = product.Name,
            Description = product.Description,
            Category = category.Name,
            YearOfRelease = product.YearOfRelease,
            ImageUrl = product.ImageUrl,
            Id = product.Id.ToString(),
            Brand = brand.Name,
            Price = product.Price,
        };

        await _publishEndpoint.Publish(_mapper.Map<ProductCreated>(productResponse));

        // All, including adding to the Outbox message table if Service Bus is down will either be added to the table
        // or not added
        var result = await _unitOfWork.Complete();

        if (result < 1)
            throw new ApiBadRequestException("An error occurred while saving changes to db");

        return productResponse;
    }
}
