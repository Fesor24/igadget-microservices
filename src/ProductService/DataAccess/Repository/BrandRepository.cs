using ProductService.Data;
using ProductService.DataAccess.Contracts;
using ProductService.Entities;

namespace ProductService.DataAccess.Repository;

public class BrandRepository : GenericRepository<Brand>,IBrandRepository
{
    public BrandRepository(ApplicationDbContext context) : base(context)
    {
        
    }
}
