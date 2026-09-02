using Microsoft.EntityFrameworkCore;
using SmartStock.Shared;

namespace SmartStock.Server.Data;

public static class StockQueries
{
    public static async Task<int> GetCurrentStockAsync(
        AppDbContext context,
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await context.StockMovements
            .Where(m => m.ProductId == productId)
            .SumAsync(m =>
                m.MovementType == StockMovementType.SaleOut
                    ? -m.Quantity
                    : m.Quantity,
                cancellationToken);
    }
}