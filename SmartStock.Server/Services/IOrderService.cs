using SmartStock.Shared;

namespace SmartStock.Server.Services;

public interface IOrderService
{
    Task<SalesOrder> PlaceSalesOrderAsync(
        int customerId,
        IReadOnlyList<PlaceOrderLine> items,
        CancellationToken cancellationToken = default);

    Task<SalesOrder?> GetOrderWithDetailsSingleQueryAsync(
        int orderId,
        CancellationToken cancellationToken = default);

    Task<SalesOrder?> GetOrderWithDetailsSplitQueryAsync(
        int orderId,
        CancellationToken cancellationToken = default);
}