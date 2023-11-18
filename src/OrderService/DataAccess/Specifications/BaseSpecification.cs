using System.Linq.Expressions;

namespace OrderService.DataAccess.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public BaseSpecification()
    {
        
    }

    public BaseSpecification(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;        
    }

    public Expression<Func<T, bool>> Criteria { get; private set; }

    public List<Expression<Func<T, object>>> Includes { get; private set; } = new();

    protected void AddIncludes(Expression<Func<T, object>> include) => 
        Includes.Add(include);
}
