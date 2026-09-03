using SmartStock.Server.Data;
using SmartStock.Shared;

namespace SmartStock.Server.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Categories = new Repository<Category>(context);
        Products = new Repository<Product>(context);
        Customers = new Repository<Customer>(context);
        Suppliers = new Repository<Supplier>(context);
    }

    public IRepository<Category> Categories { get; }
    public IRepository<Product> Products { get; }
    public IRepository<Customer> Customers { get; }
    public IRepository<Supplier> Suppliers { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}