using Microsoft.EntityFrameworkCore;
using SmartStock.Shared;

namespace SmartStock.Server.Data;

public static class SalesReportQueries
{
    // Query 1 — read the view like a normal DbSet
    public static async Task<IReadOnlyList<SalesSummaryByProduct>> GetSalesSummaryAsync(
        AppDbContext context,
        CancellationToken cancellationToken = default)
    {
        return await context.SalesSummaryByProduct
            .AsNoTracking()
            .OrderByDescending(r => r.TotalRevenue)
            .ToListAsync(cancellationToken);
    }

    // Query 2 — FromSqlInterpolated with date range (safe parameters)
    public static async Task<IReadOnlyList<SalesSummaryByProduct>> GetSalesSummaryByDateRangeAsync(
        AppDbContext context,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken = default)
    {
        return await context.SalesSummaryByProduct
            .FromSqlInterpolated($"""
                SELECT
                    p.Id AS ProductId,
                    p.Name AS ProductName,
                    SUM(soi.Quantity) AS TotalQuantitySold,
                    SUM(soi.LineTotal) AS TotalRevenue
                FROM SalesOrderItems AS soi
                INNER JOIN Products AS p ON soi.ProductId = p.Id
                INNER JOIN SalesOrders AS so ON soi.SalesOrderId = so.Id
                WHERE p.IsDeleted = 0
                  AND so.Status <> 'Cancelled'
                  AND so.OrderDate >= {fromDate}
                  AND so.OrderDate < {toDate}
                GROUP BY p.Id, p.Name
                """)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}