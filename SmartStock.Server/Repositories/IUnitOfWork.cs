using SmartStock.Shared;

namespace SmartStock.Server.Repositories;

public interface IUnitOfWork
{
    IRepository<Category> Categories { get; }
    IRepository<Product> Products { get; }
    IRepository<Customer> Customers { get; }
    IRepository<Supplier> Suppliers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}