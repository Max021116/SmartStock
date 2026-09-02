namespace SmartStock.Shared;

public class Invoice
{
    public int Id { get; set; }
    public int SalesOrderId { get; set; }      // FK lives HERE (one-to-one dependent side)
    public required string InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }

    public SalesOrder SalesOrder { get; set; } = null!;
}