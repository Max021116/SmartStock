using SmartStock.Shared;

namespace SmartStock.Server.Services;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductListDto>> GetPagedListAsync(
    int page = 1,
    int pageSize = 10,
    string? search = null,
    string sortBy = "name",      // "name", "sku", "price", "stock"
    bool sortDesc = false,
    CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductListDto>> GetPagedListBadAsync(
    CancellationToken cancellationToken = default);
}