using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DataAccess.Contracts;
using ProductService.Entities;
using ProductService.Models;
using ProductService.Response;

namespace ProductService.DataAccess.Repository;

public class ProductRepository(ProductDbContext context) : 
    GenericRepository<Product>(context), IProductRepository
{
    private readonly ProductDbContext _context = context;

    public async Task<ProductModel> GetProductDetails(Guid id) =>
        await _context.Set<Product>().Where(x => x.Id == id)
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Select(x => new ProductModel
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Brand = x.Brand.Name,
                Category = x.Category.Name,
                Id = x.Id.ToString(),
                ImageUrl = x.ImageUrl,
                YearOfRelease = x.YearOfRelease
            })
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyList<ProductModel>> GetProductsDetails() =>
        await _context.Set<Product>()
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .Select(x => new ProductModel
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                Brand = x.Brand.Name,
                Category = x.Category.Name,
                Id = x.Id.ToString(),
                ImageUrl = x.ImageUrl,
                YearOfRelease = x.YearOfRelease
            })
            .ToListAsync();
}
