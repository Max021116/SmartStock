namespace SmartStock.Shared;

public class ProductUpdateDto
{
    public decimal Price { get; set; }
    public string RowVersion { get; set; } = "";  // base64 bytes from GET
}