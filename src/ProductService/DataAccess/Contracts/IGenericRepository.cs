namespace ProductService.DataAccess.Contracts;

public interface IGenericRepository<T> where T: class
{
    Task<IReadOnlyList<T>> GetAllAsync();

    Task<T> GetAsync(Guid id);

    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);

    void DeleteAsync(T entity);
}
