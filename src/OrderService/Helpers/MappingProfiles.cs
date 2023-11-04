using AutoMapper;
using OrderService.Entities.OrderAggregate;
using OrderService.Requests;
using OrderService.Response;

namespace OrderService.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateDeliveryMethodRequest, DeliveryMethod>();
        CreateMap<DeliveryMethod, GetDeliveryMethodResponse>();
    }
}
