namespace SmartStock.Shared;

public class ProductListDto
{
    public int Id { get; set; }
    public string SKU { get; set; } = "";
    public string Name { get; set; } = "";
    public string CategoryName { get; set; } = "";
    public decimal Price { get; set; }
    public int CurrentStock { get; set; }
}