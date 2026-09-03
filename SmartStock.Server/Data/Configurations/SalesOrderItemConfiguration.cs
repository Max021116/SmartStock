using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class SalesOrderItemConfiguration
    : IEntityTypeConfiguration<SalesOrderItem>
{
    public void Configure(EntityTypeBuilder<SalesOrderItem> builder)
    {
        builder.HasKey(item => item.Id);

        builder.Property(item => item.Quantity)
            .IsRequired();

        builder.Property(item => item.UnitPrice)
            .HasPrecision(18, 2);

        builder.HasOne(item => item.Product)
            .WithMany(p => p.SalesOrderItems)
            .HasForeignKey(item => item.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable(t =>
{
    t.HasCheckConstraint("CK_SalesOrderItems_Quantity", "[Quantity] >= 0");
    t.HasCheckConstraint("CK_SalesOrderItems_UnitPrice", "[UnitPrice] >= 0");
});

        builder.Property(item => item.LineTotal)
            .HasPrecision(18, 2)
            .HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);
    }
}