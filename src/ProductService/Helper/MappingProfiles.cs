using AutoMapper;
using ProductService.Entities;
using ProductService.Models;
using ProductService.Response;
using Shared.Contracts;

namespace ProductService.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
       CreateMap<ProductModel, GetProductResponse>();

        CreateMap<Brand, GetBrandResponse>();

        CreateMap<Category, GetCategoryResponse>();

        CreateMap<GetProductResponse, ProductCreated>();
    }
}
