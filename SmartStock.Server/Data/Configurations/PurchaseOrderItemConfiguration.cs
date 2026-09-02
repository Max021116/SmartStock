using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class PurchaseOrderItemConfiguration
    : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.UnitCost)
            .HasPrecision(18, 2);

        // Product -> PurchaseOrderItem: Restrict
        builder.HasOne(item => item.Product)
            .WithMany(p => p.PurchaseOrderItems)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}