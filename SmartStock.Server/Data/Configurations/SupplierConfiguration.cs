using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class SupplierConfiguration
    : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        // Primary key
        builder.HasKey(supplier => supplier.Id);

        // Required, maximum 200 characters
        builder.Property(supplier => supplier.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Optional, maximum 1000 characters
        builder.Property(supplier => supplier.ContactEmail)
            .HasMaxLength(100);

        builder.Property(supplier => supplier.Phone)
            .HasMaxLength(20);

        builder.Property(supplier => supplier.Address)
            .HasMaxLength(200);

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasIndex(s => s.ContactEmail);
    }
}