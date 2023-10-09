using AutoMapper;
using ProductService.Entities;
using ProductService.Response;

namespace ProductService.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, GetProductResponse>()
            .ForMember(x => x.Category, o => o.MapFrom(t => t.Category.Name))
            .ForMember(x => x.Brand, o => o.MapFrom(t => t.Brand.Name));

        CreateMap<Brand, GetBrandResponse>();

        CreateMap<Category, GetCategoryResponse>();
    }
}
