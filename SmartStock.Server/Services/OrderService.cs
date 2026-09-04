using Microsoft.EntityFrameworkCore;
using SmartStock.Server.Data;
using SmartStock.Server.Exceptions;
using SmartStock.Shared;

namespace SmartStock.Server.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SalesOrder> PlaceSalesOrderAsync(
    int customerId,
    IReadOnlyList<PlaceOrderLine> items,
    CancellationToken cancellationToken = default)
    {
        // A1 — must have at least one line
        if (items is null || items.Count == 0)
            throw new ArgumentException("Order must contain at least one item.");

        // A2 — each line must be valid
        foreach (var line in items)
        {
            if (line.Quantity <= 0)
                throw new ArgumentException($"Quantity must be > 0 for product {line.ProductId}.");

            if (line.UnitPrice < 0)
                throw new ArgumentException($"Unit price must be >= 0 for product {line.ProductId}.");
        }

        // A3 — customer must exist
        var customer = await _context.Customers.FindAsync([customerId], cancellationToken);
        if (customer is null)
            throw new ArgumentException($"Customer {customerId} not found.");

        // A4 — every product must exist
        foreach (var line in items)
        {
            var product = await _context.Products.FindAsync([line.ProductId], cancellationToken);
            if (product is null)
                throw new ArgumentException($"Product {line.ProductId} not found.");
        }

        // Phase B + C start here...

        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // B — stock check INSIDE transaction
                foreach (var line in items)
                {
                    var available = await StockQueries.GetCurrentStockAsync(
                        _context, line.ProductId, cancellationToken);

                    if (line.Quantity > available)
                        throw new InsufficientStockException(
                            line.ProductId, line.Quantity, available);
                }

                // C1 — create the sales order + line items
                var order = new SalesOrder
                {
                    CustomerId = customerId,
                    OrderDate = DateTime.UtcNow,
                    Status = SalesOrderStatus.Confirmed,
                    TotalAmount = items.Sum(i => i.Quantity * i.UnitPrice),
                    Items = items.Select(i => new SalesOrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList()
                };

                _context.SalesOrders.Add(order);

                // C2 — first save: get SalesOrder.Id for FKs below
                await _context.SaveChangesAsync(cancellationToken);

                // C3 — one StockMovement (SaleOut) per line
                foreach (var line in items)
                {
                    _context.StockMovements.Add(new StockMovement
                    {
                        ProductId = line.ProductId,
                        MovementType = StockMovementType.SaleOut,
                        Quantity = line.Quantity,
                        MovementDate = DateTime.UtcNow,
                        ReferenceId = order.Id,
                        Notes = $"Sales order #{order.Id}"
                    });
                }

                // C4 — create invoice (one-to-one with order)
                _context.Invoices.Add(new Invoice
                {
                    SalesOrderId = order.Id,
                    InvoiceNumber = $"INV-{order.Id:D6}-{DateTime.UtcNow:yyyyMMdd}",
                    IssueDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(30),
                    IsPaid = false
                });

                // C5 — second save: movements + invoice
                await _context.SaveChangesAsync(cancellationToken);

                // C6 — commit: all 4 tables persisted together
                await transaction.CommitAsync(cancellationToken);

                return order;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;   // re-throw so caller sees InsufficientStockException etc.
            }
        });
    }
    public async Task<SalesOrder?> GetOrderWithDetailsSingleQueryAsync(
    int orderId,
    CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .AsNoTracking()
            .Include(o => o.Items)
            .Include(o => o.Invoice)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<SalesOrder?> GetOrderWithDetailsSplitQueryAsync(
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await _context.SalesOrders
            .AsNoTracking()
            .AsSplitQuery()
            .Include(o => o.Items)
            .Include(o => o.Invoice)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }
}