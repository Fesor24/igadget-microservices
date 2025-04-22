using MediatR;
using ProductService.DataAccess.Contracts;
using BrandEntity = ProductService.Entities.Brand;

namespace ProductService.Features.Brands.Command.Create;

internal sealed class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBrandCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        BrandEntity brand = new(Guid.NewGuid(), request.Name);

        await _unitOfWork.BrandRepository.AddAsync(brand);

        await _unitOfWork.Complete();

        return brand.Id;
    }
}
