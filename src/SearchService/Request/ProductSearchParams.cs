namespace SearchService.Request;

public record ProductSearchParams
{
    public string SearchTerm { get; set; }
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    public int YearOfReleaseStart { get; set; }
    public int YearOfReleaseEnd { get; set; }
    public decimal MinimumPrice { get; set; }
    public decimal MaximumPrice { get; set; }
    public string SortBy { get; set; } = string.Empty;
    public string SortDirection { get; set; } = string.Empty;
}
