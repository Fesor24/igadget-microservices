using Microsoft.EntityFrameworkCore;
using OrderSaga.Entities;

namespace OrderSaga;

public class OrderStateDbContext : DbContext
{
    public OrderStateDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<OrderStateData> OrderState  => Set<OrderStateData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderStateData>()
            .ToTable("OrderState", "ord")
            .HasKey(ord => ord.OrderId);
        base.OnModelCreating(modelBuilder);
    }
}