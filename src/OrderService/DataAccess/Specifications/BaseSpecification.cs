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

    public Expression<Func<T, object>> OrderByDescending { get; private set; }

    protected void AddIncludes(Expression<Func<T, object>> include) => 
        Includes.Add(include);

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc) =>
        OrderByDescending = orderByDesc;
}
