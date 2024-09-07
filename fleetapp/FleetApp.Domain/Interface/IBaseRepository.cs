using Dashdine.Domain.Context;

namespace Dashdine.Domain.Interfafe;

public interface IBaseRepository<TEntity> where TEntity : class
{
    EntityDataContext UnitOfWork { get; }
    IQueryable<TEntity> Queryable();
    void Dispose();
    Task SaveChangesAsync();
}
