using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DataAccess.Contracts;
using ProductService.Entities;

namespace ProductService.DataAccess.Repository;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Product> GetProductDetails(Guid id) =>
        await _context.Products.Where(x => x.Id == id)
            .Include(x => x.Category)
            .FirstOrDefaultAsync();
}
