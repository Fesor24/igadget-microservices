using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Entities.OrderAggregate;

namespace OrderService.Configurations;

public class DeliveryMethodEntityConfiguration : IEntityTypeConfiguration<DeliveryMethod>
{
    public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
    {
        builder.ToTable(nameof(DeliveryMethod), "ord");
        builder.HasKey(x => x.Id);
    }
}
