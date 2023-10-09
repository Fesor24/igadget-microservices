using MediatR;
using ProductService.DataAccess.Contracts;
using BrandEntity = ProductService.Entities.Brand;

namespace ProductService.Features.Brand.Command.Create;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBrandCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        BrandEntity brand = new()
        {
            Id = request.Id,
            Name = request.Name
        };

        await _unitOfWork.BrandRepository.AddAsync(brand);

        return brand.Id;
    }
}
