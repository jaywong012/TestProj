using Domain.Interfaces.IRepositories;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    IAccountRepository AccountRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}