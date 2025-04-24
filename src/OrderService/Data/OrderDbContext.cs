using Microsoft.EntityFrameworkCore;
using OrderService.Entities;
using OrderService.Entities.OrderAggregate;
using System.Reflection;
using OrderService.StateMachine;

namespace OrderService.Data;

public partial class OrderDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Order => Set<Order>();
    public DbSet<Product> Product => Set<Product>();
    public DbSet<DeliveryMethod> DeliveryMethod => Set<DeliveryMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        OnModelCreatingPartial(modelBuilder);
    }
    
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSeeding((context, _) =>
            {
                bool deliveryMethodExist = context.Set<DeliveryMethod>().Any();
                if (!deliveryMethodExist)
                {
                    context.Set<DeliveryMethod>().AddRange(_deliveryMethods);
                    context.SaveChanges();
                }
            })
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                bool deliveryMethodExist = await context.Set<DeliveryMethod>().AnyAsync();
                if (!deliveryMethodExist)
                {
                    await context.Set<DeliveryMethod>().AddRangeAsync(_deliveryMethods);
                    await context.SaveChangesAsync();
                }
            });
    }

    private List<DeliveryMethod> _deliveryMethods = [
        new DeliveryMethod(Guid.Parse("3ce1f19a-2474-43ee-a3b7-f31c41af3fac"), "Standard", "5-7 days",1000),
        new DeliveryMethod(Guid.Parse("bab14da6-79fc-448e-8955-2a0ee64fb53b"), "Express", "3-5 days", 4000),
        new DeliveryMethod(Guid.Parse("4ab56846-44f2-4efc-87c2-b4420acdcaec"), "Ultra", "24 hours", 7000)
        
    ];
}
