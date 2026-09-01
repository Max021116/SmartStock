using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartStock.Shared;

namespace SmartStock.Server.Configurations;

public class CategoryConfiguration
    : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Primary key
        builder.HasKey(category => category.Id);

        // Required, maximum 200 characters
        builder.Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Optional, maximum 1000 characters
        builder.Property(category => category.Description)
            .HasMaxLength(1000);

        // Self-referencing one-to-many relationship
        builder.HasOne(category => category.ParentCategory)
            .WithMany(category => category.SubCategories)
            .HasForeignKey(category => category.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
    new Category { Id = 1, Name = "Electronics", Description = "..." },
    new Category { Id = 2, Name = "Laptops", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 3, Name = "Desktops", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 4, Name = "Smartphones", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 5, Name = "Tablets", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 6, Name = "Wearables", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 7, Name = "Audio", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 8, Name = "Video", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 9, Name = "Gaming", ParentCategoryId = 1, Description = "..." },
    new Category { Id = 10, Name = "Office", ParentCategoryId = 1, Description = "..." }
);
    }
}