using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Entities.OrderAggregate;
using OrderService.Enums;

namespace OrderService.Configurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order), "ord");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrderStatus)
            .HasConversion(x => x.ToString(),
            c => (OrderStatus)Enum.Parse(typeof(OrderStatus), c));
        builder.Property(x => x.PaymentStatus)
            .HasConversion(x => x.ToString(),
            c => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), c)
            );
        builder.OwnsOne(x => x.DeliveryAddress, c =>
        {
            c.Property(v => v.Street).IsRequired();
            c.WithOwner();
        });
        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
