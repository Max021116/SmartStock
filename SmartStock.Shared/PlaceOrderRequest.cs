namespace SmartStock.Shared;

public class PlaceOrderRequest
{
    public int CustomerId { get; set; }
    public List<PlaceOrderLineRequest> Items { get; set; } = [];
}

public class PlaceOrderLineRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}