using Microsoft.EntityFrameworkCore;
using SmartStock.Server.Repositories;
using SmartStock.Shared;
using SmartStock.Server.Data;

namespace SmartStock.Server.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context;

    public ProductService(IUnitOfWork unitOfWork, AppDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.Products.Query().AsNoTracking().ToListAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _unitOfWork.Products.Query()
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        ValidateProduct(product);

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        ValidateProduct(product);

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, cancellationToken);
        if (product is null) return;

        _unitOfWork.Products.Delete(product);  // soft delete via SaveChangesAsync
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<ProductListDto>> GetPagedListAsync(
        int page = 1,
        int pageSize = 10,
        string? search = null,
        string sortBy = "name",
        bool sortDesc = false,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var stockByProduct = _context.StockMovements
            .GroupBy(m => m.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                CurrentStock = g.Sum(m =>
                    m.MovementType == StockMovementType.SaleOut
                        ? -m.Quantity
                        : m.Quantity)
            });
        var query = _context.Products
            .AsNoTracking()
            .Select(p => new ProductListDto
            {
                Id = p.Id,
                SKU = p.SKU,
                Name = p.Name,
                CategoryName = p.Category.Name,
                Price = p.Price,
                CurrentStock = stockByProduct
                    .Where(s => s.ProductId == p.Id)
                    .Select(s => s.CurrentStock)
                    .FirstOrDefault()
            });
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            query = query.Where(p =>
                p.Name.Contains(term) ||
                p.SKU.Contains(term) ||
                p.CategoryName.Contains(term));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        query = (sortBy.ToLowerInvariant(), sortDesc) switch
        {
            ("sku", false) => query.OrderBy(p => p.SKU),
            ("sku", true) => query.OrderByDescending(p => p.SKU),
            ("price", false) => query.OrderBy(p => p.Price),
            ("price", true) => query.OrderByDescending(p => p.Price),
            ("stock", false) => query.OrderBy(p => p.CurrentStock),
            ("stock", true) => query.OrderByDescending(p => p.CurrentStock),
            (_, true) => query.OrderByDescending(p => p.Name),
            _ => query.OrderBy(p => p.Name),
        };
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return new PagedResult<ProductListDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<IReadOnlyList<ProductListDto>> GetPagedListBadAsync(
    CancellationToken cancellationToken = default)
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .ToListAsync(cancellationToken);

        var result = new List<ProductListDto>();

        foreach (var p in products)
        {
            var stock = await StockQueries.GetCurrentStockAsync(
                _context, p.Id, cancellationToken);

            result.Add(new ProductListDto
            {
                Id = p.Id,
                SKU = p.SKU,
                Name = p.Name,
                CategoryName = p.Category.Name,
                Price = p.Price,
                CurrentStock = stock
            });
        }

        return result;
    }
    private static void ValidateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.SKU))
            throw new ArgumentException("SKU is required.");

        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.");

        if (product.Price < 0)
            throw new ArgumentException("Price must be >= 0.");
    }
}