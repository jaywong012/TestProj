using Domain.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
{
    private readonly NewProjectDbContext _context;
    private readonly DbSet<T> _dbSet;

    protected GenericRepository(NewProjectDbContext dbContext)
    {
        _context = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.Where(r => !r.IsDeleted).AsQueryable();
    }

    public async Task<T?> GetById(Guid id)
    {
        var result = await _dbSet.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        //if (result == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
        return result;
    }

    public async Task Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(r => r.Id == entity.Id && !r.IsDeleted);
        if (existingEntity == null)
        {
            throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");
        }

        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        switch (existingEntity)
        {
            case BaseModel entity:
                entity.IsDeleted = true;

                _context.Entry(existingEntity).State = EntityState.Modified;
                break;
            default:
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
        }

        await _context.SaveChangesAsync();
    }
}