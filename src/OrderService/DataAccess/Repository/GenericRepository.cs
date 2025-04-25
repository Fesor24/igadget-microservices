using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Specifications;

namespace OrderService.DataAccess.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly OrderDbContext _context;

    public GenericRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity) =>
        await _context.Set<T>().AddAsync(entity);

    public void Update(T entity)
    {
        _context.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public async Task<List<T>> GetAllAsync(ISpecification<T> spec) =>
        await ApplySpecification(spec).ToListAsync();

    public async Task<T> GetAsync(ISpecification<T> spec) =>
        await ApplySpecification(spec).FirstOrDefaultAsync();

    private IQueryable<T> ApplySpecification(ISpecification<T> spec) =>
        SpecificationEvaluator.GetQuery(_context.Set<T>().AsQueryable(), spec);
}
