namespace ProductService.Entities;

public class Product
{
    private Product()
    {
        
    }

    public Product(Guid id, string name, string description, decimal price, int yearOfRelease, 
        string imageUrl, Guid categoryId, Guid brandId)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        YearOfRelease = yearOfRelease;
        ImageUrl = imageUrl;
        CategoryId = categoryId;
        BrandId = brandId;       
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int YearOfRelease { get; private set; }
    public string ImageUrl { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public Guid BrandId { get; private set; }
    public Brand Brand { get; private set; }
    
    public void Update(string name, string description, decimal price, string imageUrl)
    {
        Name = name;
        Description = description;
        Price = price;
        ImageUrl = imageUrl;
    }
}
