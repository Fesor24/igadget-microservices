using OrderService.DataAccess.Specifications;

namespace OrderService.DataAccess.Contracts;

public interface IGenericRepository<T> where T: class
{
    Task<T> GetAsync(ISpecification<T> spec);
    Task<List<T>> GetAllAsync(ISpecification<T> spec);
    Task AddAsync(T entity);
    void Update(T entity);
}
