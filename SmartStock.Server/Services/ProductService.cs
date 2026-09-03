using Microsoft.EntityFrameworkCore;
using SmartStock.Server.Repositories;
using SmartStock.Shared;

namespace SmartStock.Server.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.Products.Query().ToListAsync(cancellationToken);

    public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _unitOfWork.Products.GetByIdAsync(id, cancellationToken);

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