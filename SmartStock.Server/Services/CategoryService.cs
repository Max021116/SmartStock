using Microsoft.EntityFrameworkCore;
using SmartStock.Server.Repositories;
using SmartStock.Shared;

namespace SmartStock.Server.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _unitOfWork.Categories.Query().ToListAsync(cancellationToken);

    public Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            throw new ArgumentException("Category name is required.");

        await _unitOfWork.Categories.AddAsync(category, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
            throw new ArgumentException("Category name is required.");

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
        if (category is null) return;

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}