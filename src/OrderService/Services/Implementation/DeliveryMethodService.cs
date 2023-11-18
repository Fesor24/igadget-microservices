using AutoMapper;
using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Specifications.Delivery;
using OrderService.Entities.OrderAggregate;
using OrderService.Requests;
using OrderService.Response;
using OrderService.Services.Contracts;
using Shared.Exceptions;

namespace OrderService.Services.Implementation;

public class DeliveryMethodService : IDeliveryMethodService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DeliveryMethodService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetDeliveryMethodResponse> CreateDeliveryMethod(CreateDeliveryMethodRequest request)
    {
        var deliveryMethod = _mapper.Map<DeliveryMethod>(request);

        await _unitOfWork.Repository<DeliveryMethod>().AddAsync(deliveryMethod);

        await _unitOfWork.Complete();

        return _mapper.Map<GetDeliveryMethodResponse>(deliveryMethod);
    }

    public async Task<GetDeliveryMethodResponse> GetDeliveryMethod(Guid deliveryMethodId)
    {
        var spec = new GetDeliverySpecification(deliveryMethodId);

        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(spec)
            ?? throw new ApiNotFoundException($"Delivery method with id: {deliveryMethodId} not found");

        return _mapper.Map<GetDeliveryMethodResponse>(deliveryMethod);
    }

    public async Task<IReadOnlyList<GetDeliveryMethodResponse>> GetDeliveryMethods()
    {
        var spec = new GetDeliverySpecification();

        var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync(spec);

        return _mapper.Map<IReadOnlyList<GetDeliveryMethodResponse>>(deliveryMethods);
    }
}
