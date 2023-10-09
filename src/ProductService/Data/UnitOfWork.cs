using ProductService.DataAccess.Contracts;
using ProductService.DataAccess.Repository;

namespace ProductService.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductDbContext _context;

    public UnitOfWork(ProductDbContext context)
    {
        ProductRepository = new ProductRepository(context);
        BrandRepository = new BrandRepository(context);
        CategoryRepository = new CategoryRepository(context);
        _context = context;
    }

    public IProductRepository ProductRepository { get; private set; }

    public IBrandRepository BrandRepository { get; private set; }

    public ICategoryRepository CategoryRepository { get; private set; }

    public async Task<int> Complete() =>
        await _context.SaveChangesAsync();

    public void Dispose() =>
        _context.Dispose();
}
