using ProductService.Entities;

namespace ProductService.DataAccess.Contracts;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<Product> GetProductDetails(Guid id);
    Task<IReadOnlyList<Product>> GetProductsDetails();
}
