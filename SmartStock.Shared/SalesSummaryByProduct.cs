namespace SmartStock.Shared;

public class SalesSummaryByProduct
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int TotalQuantitySold { get; set; }
    public decimal TotalRevenue { get; set; }
}