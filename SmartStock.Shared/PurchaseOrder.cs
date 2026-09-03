namespace SmartStock.Shared;

public class PurchaseOrder
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public byte[] RowVersion { get; set; } = null!;
    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}