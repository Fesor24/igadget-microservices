using Microsoft.EntityFrameworkCore;
using OrderService.Entities;
using OrderService.Entities.OrderAggregate;
using System.Reflection;

namespace OrderService.Data;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Order> Order => Set<Order>();

    public DbSet<Product> Product => Set<Product>();
    public DbSet<DeliveryMethod> DeliveryMethod => Set<DeliveryMethod>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
