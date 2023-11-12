using AutoMapper;
using MediatR;
using ProductService.DataAccess.Contracts;
using ProductService.Response;

namespace ProductService.Features.Brand.Queries.GetBrands;

internal sealed class GetBrandsRequestHandler : IRequestHandler<GetBrandsRequest, IReadOnlyList<GetBrandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBrandsRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GetBrandResponse>> Handle(GetBrandsRequest request, 
        CancellationToken cancellationToken)
    {
        var brands = await _unitOfWork.BrandRepository.GetAllAsync();

        return _mapper.Map<IReadOnlyList<GetBrandResponse>>(brands);
    }
}
