namespace ProductService.Entities;

public sealed class Brand
{
    private Brand()
    {
        
    }

    public Brand(Guid id, string name)
    {
        Id = id;
        Name = name;       
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public List<Product> Products { get; private set; } = [];
}
