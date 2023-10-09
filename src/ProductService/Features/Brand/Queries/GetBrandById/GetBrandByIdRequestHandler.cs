using AutoMapper;
using MediatR;
using ProductService.DataAccess.Contracts;
using ProductService.Response;
using Shared.Exceptions;

namespace ProductService.Features.Brand.Queries.GetBrandById;

public class GetBrandByIdRequestHandler : IRequestHandler<GetBrandByIdRequest, GetBrandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBrandByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetBrandResponse> Handle(GetBrandByIdRequest request, CancellationToken cancellationToken)
    {
        var brand = await _unitOfWork.BrandRepository.GetAsync(request.Id);

        return brand is null ? throw new ApiNotFoundException($"Brand with Id: {request.Id} not found") :
            _mapper.Map<GetBrandResponse>(brand);
    }
}
