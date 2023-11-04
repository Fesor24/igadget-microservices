using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Entities.OrderAggregate;
using OrderService.Requests;
using OrderService.Response;
using OrderService.Services.Contracts;
using Shared.Exceptions;

namespace OrderService.Services.Implementation;

public class DeliveryMethodService : IDeliveryMethodService
{
    private readonly OrderDbContext _context;
    private readonly IMapper _mapper;

    public DeliveryMethodService(OrderDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetDeliveryMethodResponse> CreateDeliveryMethod(CreateDeliveryMethodRequest request)
    {
        var deliveryMethod = _mapper.Map<DeliveryMethod>(request);

        await _context.DeliveryMethod.AddAsync(deliveryMethod);

        await _context.SaveChangesAsync();

        return _mapper.Map<GetDeliveryMethodResponse>(deliveryMethod);
    }

    public async Task<GetDeliveryMethodResponse> GetDeliveryMethod(Guid deliveryMethodId)
    {
        var deliveryMethod = await _context.DeliveryMethod
            .FirstOrDefaultAsync(x => x.Id == deliveryMethodId) ??
            throw new ApiNotFoundException($"Delivery method with id: {deliveryMethodId} not found");

        return _mapper.Map<GetDeliveryMethodResponse>(deliveryMethod);
    }

    public async Task<IReadOnlyList<GetDeliveryMethodResponse>> GetDeliveryMethods()
    {
        var deliveryMethods = await _context.DeliveryMethod.ToListAsync();

        return _mapper.Map<IReadOnlyList<GetDeliveryMethodResponse>>(deliveryMethods);
    }
}
