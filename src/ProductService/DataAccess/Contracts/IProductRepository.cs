using ProductService.Entities;
using ProductService.Models;

namespace ProductService.DataAccess.Contracts;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<ProductModel> GetProductDetails(Guid id);
    Task<IReadOnlyList<ProductModel>> GetProductsDetails();
}
