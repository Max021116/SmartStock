namespace SmartStock.Server.Exceptions;

public class InsufficientStockException : Exception
{
    public int ProductId { get; }
    public int Requested { get; }
    public int Available { get; }

    public InsufficientStockException(int productId, int requested, int available)
        : base($"Product {productId}: requested {requested}, only {available} in stock.")
    {
        ProductId = productId;
        Requested = requested;
        Available = available;
    }
}