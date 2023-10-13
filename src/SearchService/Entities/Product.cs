using MongoDB.Entities;

namespace SearchService.Entities;

public class Product : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int YearOfRelease { get; set; }
    public string ImageUrl { get; set; }
    public string CategoryName { get; set; }
    public string BrandName { get; set; }
}
