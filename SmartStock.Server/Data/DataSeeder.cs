using Microsoft.EntityFrameworkCore;
using SmartStock.Shared;

namespace SmartStock.Server.Data;

public static class DataSeeder
{
    public static async Task SeedDevelopmentDataAsync(AppDbContext context)
    {
        // Skip if already seeded
        if (await context.Customers.AnyAsync())
            return;

        // Customers
        context.Customers.AddRange(
            new Customer { Name = "Acme Corp", Email = "buyer@acme.com", Phone = "555-0100", Address = "123 Main St" },
            new Customer { Name = "Globex Ltd", Email = "orders@globex.com", Phone = "555-0200", Address = "456 Oak Ave" },
            new Customer { Name = "Initech", Email = "procurement@initech.com", Phone = "555-0300", Address = "789 Pine Rd" }
        );

        // Suppliers
        context.Suppliers.AddRange(
            new Supplier { Name = "TechSupply Co", ContactEmail = "sales@techsupply.com", Phone = "555-1000", Address = "100 Industrial Blvd" },
            new Supplier { Name = "PartsDirect", ContactEmail = "info@partsdirect.com", Phone = "555-2000", Address = "200 Warehouse Ln" }
        );

        await context.SaveChangesAsync();

        // Stock for first 3 products (PurchaseIn)
        var productIds = await context.Products
            .OrderBy(p => p.Id)
            .Take(3)
            .Select(p => p.Id)
            .ToListAsync();

        foreach (var productId in productIds)
        {
            context.StockMovements.Add(new StockMovement
            {
                ProductId = productId,
                MovementType = StockMovementType.PurchaseIn,
                Quantity = 500,
                MovementDate = DateTime.UtcNow,
                Notes = "Development seed stock"
            });
        }

        await context.SaveChangesAsync();
    }
}