namespace SmartStock.Shared;

public class Supplier
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? ContactEmail { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
}