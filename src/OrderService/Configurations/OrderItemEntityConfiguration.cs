using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Entities.OrderAggregate;

namespace OrderService.Configurations;

public class OrderItemEntityConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable(nameof(OrderItem), "ord");
        builder.HasKey(x => x.Id);
        builder.OwnsOne(x => x.ItemOrdered, c =>
        {
            c.Property(x => x.Name).IsRequired();
            c.Property(x => x.ProductId).IsRequired();
            c.WithOwner();
        });
    }
}
