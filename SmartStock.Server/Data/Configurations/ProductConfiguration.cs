using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class ProductConfiguration
    : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Primary key
        builder.HasKey(product => product.Id);

        // SKU is required and limited to 100 characters
        builder.Property(product => product.SKU)
            .IsRequired()
            .HasMaxLength(100);

        // SKU must be unique
        builder.HasIndex(product => product.SKU)
            .IsUnique();

        // Name is required and limited to 200 characters
        builder.Property(product => product.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Description is optional
        builder.Property(product => product.Description)
            .HasMaxLength(1000);

        // SQL decimal(18, 2)
        builder.Property(product => product.Price)
            .HasPrecision(18, 2);

        builder.Property(product => product.IsActive)
            .IsRequired();

        builder.Property(product => product.CreatedAt)
            .IsRequired();

        // One Category has many Products
        builder.HasOne(product => product.Category)
            .WithMany(category => category.Products)
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
    new Product { Id = 1, SKU = "SKU-001", Name = "Product 1", CategoryId = 2, Price = 999.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 2, SKU = "SKU-002", Name = "Product 2", CategoryId = 2, Price = 899.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 3, SKU = "SKU-003", Name = "Product 3", CategoryId = 2, Price = 799.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 4, SKU = "SKU-004", Name = "Product 4", CategoryId = 2, Price = 699.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 5, SKU = "SKU-005", Name = "Product 5", CategoryId = 2, Price = 599.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 6, SKU = "SKU-006", Name = "Product 6", CategoryId = 2, Price = 499.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 7, SKU = "SKU-007", Name = "Product 7", CategoryId = 2, Price = 399.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 8, SKU = "SKU-008", Name = "Product 8", CategoryId = 2, Price = 299.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 9, SKU = "SKU-009", Name = "Product 9", CategoryId = 2, Price = 199.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." },
    new Product { Id = 10, SKU = "SKU-010", Name = "Product 10", CategoryId = 2, Price = 99.99m, IsActive = true, CreatedAt = new DateTime(2026, 1, 1), Description = "..." }
);
    }
}