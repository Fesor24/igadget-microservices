namespace ProductService.Request;

public class UpdateProductRequest
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
}
