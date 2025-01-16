using Domain.Interfaces.IRepositories;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}