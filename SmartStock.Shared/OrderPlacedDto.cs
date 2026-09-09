namespace SmartStock.Shared;

public class OrderPlacedDto
{
    public int OrderId { get; set; }
    public decimal TotalAmount { get; set; }
    public string InvoiceNumber { get; set; } = "";
}