using ProductService.Data;
using ProductService.DataAccess.Contracts;
using ProductService.Entities;

namespace ProductService.DataAccess.Repository;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ProductDbContext context) : base(context)
    {
        
    }
}
