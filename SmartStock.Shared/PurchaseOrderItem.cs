namespace SmartStock.Shared;

public class PurchaseOrderItem
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }
    public Product Product { get; set; } = null!;
    public PurchaseOrder PurchaseOrder { get; set; } = null!;
}