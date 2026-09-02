using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class InvoiceConfiguration
    : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        builder.Property(i => i.IssueDate)
            .IsRequired();

        builder.Property(i => i.DueDate)
            .IsRequired();

        // One-to-one: FK on Invoice side
        builder.HasOne(i => i.SalesOrder)
            .WithOne(so => so.Invoice)
            .HasForeignKey<Invoice>(i => i.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique FK = enforces one invoice per order
        builder.HasIndex(i => i.SalesOrderId)
            .IsUnique();
    }
}