using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class PurchaseOrderConfiguration
    : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        builder.HasKey(po => po.Id);

        builder.Property(po => po.OrderDate)
            .IsRequired();

        builder.Property(po => po.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(po => po.TotalAmount)
            .HasPrecision(18, 2);

        // Supplier -> PurchaseOrder: Restrict
        builder.HasOne(po => po.Supplier)
            .WithMany(s => s.PurchaseOrders)
            .HasForeignKey(po => po.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        // PurchaseOrder -> Items: Cascade
        builder.HasMany(po => po.Items)
            .WithOne(item => item.PurchaseOrder)
            .HasForeignKey(item => item.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}