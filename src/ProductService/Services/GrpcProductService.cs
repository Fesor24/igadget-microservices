using Grpc.Core;
using ProductService.DataAccess.Contracts;

namespace ProductService.Services;

public class GrpcProductService : GrpcProduct.GrpcProductBase
{
    private readonly IUnitOfWork _unitOfWork;

    public GrpcProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<GrpcProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _unitOfWork.ProductRepository.GetProductDetails(Guid.Parse(request.Id));

        if (product is null) return null;

        var prdResponse = new GrpcProductResponse
        {
            Price = (float)product.Price,
            Name = product.Name,
            Brand = product.Brand.Name,
            Category = product.Category.Name,
            ImageUrl = product.ImageUrl,
            Id = product.Id.ToString()
        };

        return prdResponse;
    }
}
