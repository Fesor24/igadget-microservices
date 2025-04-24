using Microsoft.EntityFrameworkCore;
using OrderSaga.Entities;

namespace OrderSaga;

public class OrderStateDbContext : DbContext
{
    public OrderStateDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<OrderStateData> OrderState  => Set<OrderStateData>();
}