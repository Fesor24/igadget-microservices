using AutoMapper;
using MediatR;
using ProductService.DataAccess.Contracts;
using ProductService.Response;

namespace ProductService.Features.Category.Queries.GetCategories;

internal sealed class GetCategoriesRequestHandler : IRequestHandler<GetCategoriesRequest, 
    IReadOnlyList<GetCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesRequestHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<GetCategoryResponse>> Handle(GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.CategoryRepository.GetAllAsync();

        return _mapper.Map<IReadOnlyList<GetCategoryResponse>>(categories);
    }
}
