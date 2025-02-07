using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Repositories;

namespace Infrastructure.Configurations;

public class UnitOfWork(NewProjectDbContext dbContext) : IUnitOfWork, IDisposable
{
    private readonly NewProjectDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;
    private IAccountRepository? _accountRepository;
    private IPostRepository? _postRepository;
    private IAccountPostShareRepository? _accountPostShareRepository;
    private ISocialAccessInfoRepository? _socialAccessInfoRepository;

    public IProductRepository ProductRepository =>
        _productRepository ??= new ProductRepository(_dbContext);

    public ICategoryRepository CategoryRepository =>
        _categoryRepository ??= new CategoryRepository(_dbContext);

    public IAccountRepository AccountRepository =>
        _accountRepository ??= new AccountRepository(_dbContext);

    public IPostRepository PostRepository =>
        _postRepository ??= new PostRepository(_dbContext);

    public IAccountPostShareRepository AccountPostShareRepository =>
        _accountPostShareRepository ??= new AccountPostShareRepository(_dbContext);

    public ISocialAccessInfoRepository SocialAccessInfoRepository =>
        _socialAccessInfoRepository ??= new SocialAccessInfoRepository(_dbContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}