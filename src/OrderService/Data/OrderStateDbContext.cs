using Microsoft.EntityFrameworkCore;
using OrderService.StateMachine;

namespace OrderService.Data;

public partial class OrderDbContext : DbContext
{
    public DbSet<OrderStateData> OrderState => Set<OrderStateData>();

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderStateData>()
            .ToTable("OrderState", "ord")
            .HasKey(ord => ord.OrderId);

        // if using optimistic concurrency...
        //modelBuilder.Entity<OrderStateData>()
          //  .Property(x => x.RowVersion)
            //.IsRowVersion(); 
        
        base.OnModelCreating(modelBuilder);
    }
}