namespace ProductService.DataAccess.Contracts;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }

    IBrandRepository BrandRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    Task<int> Complete();
}
