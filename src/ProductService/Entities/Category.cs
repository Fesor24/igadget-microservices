namespace ProductService.Entities;

public class Category
{
    private Category()
    {
        
    }

    public Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public List<Product> Products { get; private set; } = [];
}
