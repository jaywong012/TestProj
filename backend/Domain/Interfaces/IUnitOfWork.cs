using Domain.Interfaces.IRepositories;

namespace Domain.Interfaces;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }

    ICategoryRepository CategoryRepository { get; }

    IAccountRepository AccountRepository { get; }

    IPostRepository PostRepository { get; }

    IAccountPostShareRepository AccountPostShareRepository { get; }

    ISocialAccessInfoRepository SocialAccessInfoRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}