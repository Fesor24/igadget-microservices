using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.DataAccess.Contracts;

namespace ProductService.DataAccess.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ProductDbContext _context;

    public GenericRepository(ProductDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity) => 
        await _context.Set<T>().AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<T> entities) =>
        await _context.Set<T>().AddRangeAsync(entities);

    public void DeleteAsync(T entity) =>
        _context.Set<T>().Remove(entity);

    public async Task<IReadOnlyList<T>> GetAllAsync() =>
        await _context.Set<T>().ToListAsync();

    public async Task<T> GetAsync(Guid id) => 
        await _context.Set<T>().FindAsync(id);

    public void Update(T entity)
    {
        _context.Attach(entity);

        _context.Entry(entity).State = EntityState.Modified;
    }
}
