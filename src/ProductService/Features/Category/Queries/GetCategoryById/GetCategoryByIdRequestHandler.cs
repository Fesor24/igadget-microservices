using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using ProductService.DataAccess.Contracts;
using ProductService.Response;
using Shared.Exceptions;

namespace ProductService.Features.Category.Queries.GetCategoryById;

internal sealed class GetCategoryByIdRequestHandler : IRequestHandler<GetCategoryByIdRequest, GetCategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoryByIdRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<GetCategoryResponse> Handle(GetCategoryByIdRequest request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepository.GetAsync(request.Id);

        return category is null ?
            throw new ApiNotFoundException($"Category with Id: {request.Id} not found") :
            _mapper.Map<GetCategoryResponse>(category);
    }
}
