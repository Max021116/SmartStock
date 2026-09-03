namespace SmartStock.Shared;

public interface IAuditable
{
    bool IsDeleted { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    string? CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
}