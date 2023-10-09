using AutoMapper;
using MediatR;
using ProductService.DataAccess.Contracts;
using ProductService.Response;
using Shared.Exceptions;

namespace ProductService.Features.Product.Queries.GetProductById;

public class GetProductByIdRequestHandler : IRequestHandler<GetProductByIdRequest, GetProductResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProductByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetProductResponse> Handle(GetProductByIdRequest request,
        CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetProductDetails(request.Id);

        return product is null
            ? throw new ApiNotFoundException($"Product with Id: {request.Id} not found")
            : _mapper.Map<GetProductResponse>(product);
    }
}
