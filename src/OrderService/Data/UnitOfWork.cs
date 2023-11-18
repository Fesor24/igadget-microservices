using OrderService.DataAccess.Contracts;
using OrderService.DataAccess.Repository;
using System.Collections;

namespace OrderService.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly OrderDbContext _context;

    private Hashtable _repositories;

    public UnitOfWork(OrderDbContext context) => _context = context;

    public async Task<int> Complete() => 
        await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        _repositories ??= new Hashtable();

        var key = typeof(TEntity).Name;

        if(!_repositories.ContainsKey(key)) 
        {
            var genericType = typeof(GenericRepository<>);

            var instance = Activator.CreateInstance(genericType.MakeGenericType(typeof(TEntity)), _context);

            _repositories.Add(key, instance);
        }

        return (IGenericRepository<TEntity>)_repositories[key];
    }
}
