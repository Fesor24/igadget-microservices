namespace ProductService.DataAccess.Contracts;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }

    IBrandRepository BrandRepository { get; }

    Task<int> Complete();
}
