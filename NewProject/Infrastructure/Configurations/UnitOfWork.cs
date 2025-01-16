using Domain.Interfaces;
using Domain.Interfaces.IRepositories;
using Infrastructure.Repositories;

namespace Infrastructure.Configurations;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly NewProjectDbContext _dbContext;
    private IProductRepository? _productRepository;
    private ICategoryRepository? _categoryRepository;

    public UnitOfWork(NewProjectDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IProductRepository ProductRepository =>
        _productRepository ??= new ProductRepository(_dbContext);

    public ICategoryRepository CategoryRepository =>
        _categoryRepository ??= new CategoryRepository(_dbContext);

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