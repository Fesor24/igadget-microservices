using Microsoft.EntityFrameworkCore;

namespace OrderService.DataAccess.Specifications;
public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> input, ISpecification<T> spec) where T: class
    {
        var query = input.AsQueryable();

        if(spec.Criteria is not null)
        {
            query = query.Where(spec.Criteria);
        }

        if(spec.OrderByDescending is not null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        query = spec.Includes.Aggregate(query, (query, include) => query.Include(include));

        return query;
    }
}
