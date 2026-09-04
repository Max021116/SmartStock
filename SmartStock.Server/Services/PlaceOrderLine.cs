namespace SmartStock.Server.Services;

public record PlaceOrderLine(int ProductId, int Quantity, decimal UnitPrice);