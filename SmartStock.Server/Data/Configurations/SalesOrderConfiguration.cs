using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class SalesOrderConfiguration
    : IEntityTypeConfiguration<SalesOrder>
{
    public void Configure(EntityTypeBuilder<SalesOrder> builder)
    {
        builder.HasKey(so => so.Id);

        builder.Property(so => so.OrderDate)
            .IsRequired();

        builder.Property(so => so.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(so => so.TotalAmount)
            .HasPrecision(18, 2);

        builder.HasOne(so => so.Customer)
            .WithMany(c => c.SalesOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(so => so.Items)
            .WithOne(item => item.SalesOrder)
            .HasForeignKey(item => item.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(po => po.RowVersion)
            .IsRowVersion();
            
        
    }
}