using AutoMapper;
using SearchService.Entities;
using Shared.Contracts;

namespace SearchService.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<ProductCreated, Product>();
    }
}
