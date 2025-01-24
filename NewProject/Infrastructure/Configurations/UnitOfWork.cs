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

    public IProductRepository ProductRepository =>
        _productRepository ??= new ProductRepository(_dbContext);

    public ICategoryRepository CategoryRepository =>
        _categoryRepository ??= new CategoryRepository(_dbContext);

    public IAccountRepository AccountRepository => 
        _accountRepository ??= new AccountRepository(_dbContext);

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