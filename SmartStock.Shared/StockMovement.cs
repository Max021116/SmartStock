namespace SmartStock.Shared;

public class StockMovement
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public StockMovementType MovementType { get; set; }
    public int Quantity { get; set; }
    public DateTime MovementDate { get; set; }
    public int? ReferenceId { get; set; }
    public string? Notes { get; set; }
    public Product Product { get; set; } = null!;
}