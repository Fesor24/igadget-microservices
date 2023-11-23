using MongoDB.Entities;
using SearchService.Entities;
using SearchService.Request;
using SearchService.Response;

namespace SearchService.Endpoints;

public static class SearchEndpoint
{
    public static void Register(WebApplication app)
    {
        var search = app.MapGroup("api/Search");

        search.MapGet("/", SearchProducts)
            .WithTags("Search");
           
    }

    private static async Task<IResult> SearchProducts([AsParameters] ProductSearchParams searchParams)
    {
        var query = DB.PagedSearch<Product, GetProductResponse>();

        if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
        {
            string search = searchParams.SearchTerm.Replace("_", " ");

            query.Match(Search.Full, search).SortByTextScore();
        }

        if(searchParams.YearOfReleaseStart != default)
        {
            query.Match(x => x.YearOfRelease >= searchParams.YearOfReleaseStart);
        }

        if(searchParams.YearOfReleaseEnd != default)
        {
            query.Match(x => x.YearOfRelease <= searchParams.YearOfReleaseEnd);
        }

        if(searchParams.MinimumPrice != default)
            query.Match(x => x.Price >=  searchParams.MinimumPrice);

        if (searchParams.MaximumPrice != default)
            query.Match(x => x.Price <= searchParams.MaximumPrice);

        if(!string.IsNullOrWhiteSpace(searchParams.SortBy) && string.IsNullOrWhiteSpace(searchParams.SortDirection))
        {
            searchParams.SortDirection = "asc";
        }

        query = (searchParams.SortBy, searchParams.SortDirection) switch
        {
            ("Category", "desc") => query.Sort(x => x.Category, Order.Descending),
            ("Category", "asc") => query.Sort(x => x.Category, Order.Ascending),
            ("Brand", "desc") => query.Sort(x => x.Brand, Order.Descending),
            ("Brand", "asc") => query.Sort(x => x.Brand, Order.Ascending),
            ("Price", "asc") => query.Sort(x => x.Price, Order.Ascending),
            ("Price", "desc") => query.Sort(x => x.Price, Order.Descending),
            _ => query.Sort(x => x.Name, Order.Ascending)
        };

        var result = await query
            .Project(x => new GetProductResponse(
                x.ID, 
                x.Name, 
                x.Description, 
                x.Price, 
                x.YearOfRelease, 
                x.ImageUrl, 
                x.Category, 
                x.Brand))
            .PageSize(searchParams.PageSize)
            .PageNumber(searchParams.PageNumber)
            .ExecuteAsync();


        return Results.Ok(new
        {
            results = result.Results,
            result.TotalCount,
            result.PageCount
        });
    }
}
