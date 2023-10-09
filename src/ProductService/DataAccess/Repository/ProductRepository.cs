using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DataAccess.Contracts;
using ProductService.Entities;

namespace ProductService.DataAccess.Repository;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly ProductDbContext _context;

    public ProductRepository(ProductDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Product> GetProductDetails(Guid id) =>
        await _context.Products.Where(x => x.Id == id)
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyList<Product>> GetProductsDetails() =>
        await _context.Products
            .Include(x => x.Category)
            .Include(x => x.Brand)
            .ToListAsync();
}
