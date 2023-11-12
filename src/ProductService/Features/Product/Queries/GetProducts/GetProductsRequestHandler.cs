using AutoMapper;
using MediatR;
using ProductService.DataAccess.Contracts;
using ProductService.Response;

namespace ProductService.Features.Product.Queries.GetProducts;

internal sealed class GetProductsRequestHandler : IRequestHandler<GetProductsRequest, IReadOnlyList<GetProductResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductsRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GetProductResponse>> Handle(GetProductsRequest request, CancellationToken cancellationToken)
    {
        var products = await _unitOfWork.ProductRepository.GetProductsDetails();

        return _mapper.Map<IReadOnlyList<GetProductResponse>>(products);
    }
}
