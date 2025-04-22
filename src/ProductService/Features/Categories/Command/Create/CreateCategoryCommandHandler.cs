using MediatR;
using ProductService.DataAccess.Contracts;
using CategoryEntity =  ProductService.Entities.Category;

namespace ProductService.Features.Categories.Command.Create;

internal sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        CategoryEntity category = new(Guid.NewGuid(), request.Name);

        await _unitOfWork.CategoryRepository.AddAsync(category);

        await _unitOfWork.Complete();

        return category.Id;
    }
}
