using Domain.Base;

namespace Domain.Interfaces;

public interface IGenericRepository<T> where T : BaseModel
{
    IQueryable<T> GetAll();

    Task<T?> GetById(Guid id);

    Task Add(T entity);

    Task Update(T entity);

    Task Delete(Guid id);

}