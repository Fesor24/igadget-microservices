using ProductService.Data;
using ProductService.Entities;

namespace ProductService.IntegrationTests.Utils;
public static class Database
{
    public static void InitializeDb(ProductDbContext context)
    {
        context.Brands.AddRange(GetBrands());
        context.Categories.AddRange(GetCategories());
        context.SaveChanges();
        context.Products.AddRange(GetProducts());
        context.SaveChanges();
    }

    public static void ReInitializeDb(ProductDbContext context)
    {
        context.Products.RemoveRange(context.Products);
        context.Brands.RemoveRange(context.Brands);
        context.Categories.RemoveRange(context.Categories);
        context.SaveChanges();
        InitializeDb(context);
    }

    private static List<Product> GetProducts() =>
        new List<Product>
        {
            new Product
            {
                Id = Guid.Parse("47e7f8a8-4e1f-4377-9bb3-be80f340c1aa"),
                Name = "Apple Iphone 12",
                Description = "Apple Iphone 12",
                Price = 400500.70m,
                YearOfRelease = 2020,
                ImageUrl = "",
                CategoryId = Guid.Parse("56b12de0-76ad-4e1f-834f-e9b418578a98"),
                BrandId = Guid.Parse("a9effcdf-90cd-488d-84e0-4f72d0a0cc16")
            },
             new Product
            {
                Id = Guid.Parse("3e381e10-4be5-4406-ba23-3bd323b70710"),
                Name = "Samsung s22",
                Description = "Samsung s22",
                Price = 530500.70m,
                YearOfRelease = 2022,
                ImageUrl = "",
                CategoryId = Guid.Parse("e8b83e8d-deff-4228-9c03-1828d233eb2e"),
                BrandId = Guid.Parse("18545394-3e6f-40e5-8397-6dd4f09ea545")
            }
        };

    private static List<Category> GetCategories() =>
        new()
        {
            new Category{Id = Guid.Parse("e8b83e8d-deff-4228-9c03-1828d233eb2e"), Name = "Android" },
            new Category{Id = Guid.Parse("56b12de0-76ad-4e1f-834f-e9b418578a98"), Name = "IPhone" }
        };

    private static List<Brand> GetBrands() =>
        new()
        {
            new Brand{Id = Guid.Parse("a9effcdf-90cd-488d-84e0-4f72d0a0cc16"), Name = "Apple" },
            new Brand{Id = Guid.Parse("18545394-3e6f-40e5-8397-6dd4f09ea545"), Name = "Samsung" }
        };
}
