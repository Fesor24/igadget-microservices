namespace SearchService.Response;

public record GetProductResponse(
    string Id, 
    string Name, 
    string Description,
    decimal Price,
    int YearOfRelease,
    string ImageUrl,
    string Category,
    string Brand);
