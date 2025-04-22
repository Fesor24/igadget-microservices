using Microsoft.EntityFrameworkCore;
using ProductService.Entities;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit;

namespace ProductService.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Product> Product => Set<Product>();

    public DbSet<Brand> Brand => Set<Brand>();

    public DbSet<Category> Category => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) =>
        {
            bool categoryExist = context.Set<Category>().Any();
            if (!categoryExist)
            {
                context.Set<Category>().AddRange(_categories);
                context.SaveChanges();
            }
            
            bool brandExist = context.Set<Brand>().Any();
            if (!brandExist)
            {
                context.Set<Brand>().AddRange(_brands);
                context.SaveChanges();
            }
            
            bool productExist = context.Set<Product>().Any();
            if (!productExist)
            {
                context.Set<Product>().AddRange(_products);
                context.SaveChanges();
            }
        })
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                bool categoryExist = await context.Set<Category>().AnyAsync();
                if (!categoryExist)
                {
                    await context.Set<Category>().AddRangeAsync(_categories);
                    await context.SaveChangesAsync(cancellationToken);
                }
            
                bool brandExist = await context.Set<Brand>().AnyAsync();
                if (!brandExist)
                {
                    await context.Set<Brand>().AddRangeAsync(_brands);
                    await context.SaveChangesAsync(cancellationToken);
                }
            
                bool productExist = await context.Set<Product>().AnyAsync();
                if (!productExist)
                {
                    await context.Set<Product>().AddRangeAsync(_products);
                    await context.SaveChangesAsync(cancellationToken);
                }
            });
    }

    private readonly List<Category> _categories = [
        new Category(Guid.Parse("4f509c27-a929-4bc7-86f7-2b7be3b70cb6"), "Airpods"),
        new Category(Guid.Parse("1732fd2f-9ebb-4d67-b325-a57f2e5aa11d"), "Phones"),
        new Category(Guid.Parse("63d7b756-07bd-4e44-b840-4ad08e9864ab"), "Laptops"),
        new Category(Guid.Parse("811baf74-57f1-4324-8e00-038a2fbd8e2e"), "TVs"),
    ];

    private readonly List<Brand> _brands = [
        new Brand(Guid.Parse("58ac0579-8b78-4306-8b4e-95a320a8b8e9"), "Apple"),
        new Brand(Guid.Parse("0cc9c511-4373-47c5-80cb-a68860029b68"), "Samsung"),
        new Brand(Guid.Parse("212db38f-f390-482a-b1dc-174f3b6be6bc"), "LG"),
        new Brand(Guid.Parse("34d50aac-5cb1-4808-b31c-be97281787ab"), "HP")
    ];

    private readonly List<Product> _products =
    [
        new Entities.Product(Guid.Parse("cb7f4bff-e3a5-4d54-befa-75e2766cff8f"), "32 Inches LG TV",
            "32 inches LG TV", 400000, 2024, "",
            Guid.Parse("811baf74-57f1-4324-8e00-038a2fbd8e2e"), Guid.Parse("212db38f-f390-482a-b1dc-174f3b6be6bc")),
        new Entities.Product(Guid.Parse("e63cfc27-03e4-4927-9ed8-34d3f7553b3a"), "Iphone 12",
            "Apple IPhone 12", 500000, 2025, "",
            Guid.Parse("1732fd2f-9ebb-4d67-b325-a57f2e5aa11d"), Guid.Parse("58ac0579-8b78-4306-8b4e-95a320a8b8e9")),
        new Entities.Product(Guid.Parse("be728c19-b4e7-4694-82ff-8c203246b815"), "Samsung S15",
            "Samsung S15 gaming phone ultra", 800000, 2025, "",
            Guid.Parse("1732fd2f-9ebb-4d67-b325-a57f2e5aa11d"), Guid.Parse("0cc9c511-4373-47c5-80cb-a68860029b68")),
        new Entities.Product(Guid.Parse("a09c828c-527a-4887-a597-40b770926523"), "93 Inches Samsung TV",
            "93 inches Samsung TV", 900000, 2024, "",
            Guid.Parse("811baf74-57f1-4324-8e00-038a2fbd8e2e"), Guid.Parse("0cc9c511-4373-47c5-80cb-a68860029b68")),
        new Entities.Product(Guid.Parse("4aede91c-a2c6-4664-8295-6a52de4a65ae"), "Samsung S6",
            "Samsung s6 refurbished", 80000, 2016, "",
            Guid.Parse("1732fd2f-9ebb-4d67-b325-a57f2e5aa11d"), Guid.Parse("0cc9c511-4373-47c5-80cb-a68860029b68")),
        new Entities.Product(Guid.Parse("d38caade-01f8-4ba9-be2e-84e8b3874c70"), "Airpods Pro Max",
            "Apple Airpods pro max", 130000, 2024, "",
            Guid.Parse("4f509c27-a929-4bc7-86f7-2b7be3b70cb6"), Guid.Parse("58ac0579-8b78-4306-8b4e-95a320a8b8e9")),
        new Entities.Product(Guid.Parse("1e065830-5b77-49b8-82b5-dee1916a8507"), "HP EliteBook",
            "HP EliteBook G902", 630000, 2024, "",
            Guid.Parse("63d7b756-07bd-4e44-b840-4ad08e9864ab"), Guid.Parse("34d50aac-5cb1-4808-b31c-be97281787ab")),
    ];
}
