using AutoMapper;
using ProductService.Entities;
using ProductService.Response;

namespace ProductService.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, GetProductResponse>()
            .ForMember(x => x.CategoryName, o => o.MapFrom(t => t.Category.Name));

        CreateMap<Brand, GetBrandResponse>();
    }
}
