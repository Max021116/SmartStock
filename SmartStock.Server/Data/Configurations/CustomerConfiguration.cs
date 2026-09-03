using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class CustomerConfiguration
    : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Primary key
        builder.HasKey(customer => customer.Id);

        // Required, maximum 200 characters
        builder.Property(customer => customer.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Optional, maximum 1000 characters
        builder.Property(customer => customer.Email)
            .HasMaxLength(100);

        builder.Property(customer => customer.Phone)
            .HasMaxLength(20);

        builder.Property(customer => customer.Address)
            .HasMaxLength(200);
        
        builder.HasQueryFilter(c => !c.IsDeleted);
    }
}