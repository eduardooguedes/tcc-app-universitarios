using Dashdine.Domain.Context;
using Dashdine.Domain.Interfafe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dashdine.Infrastructure.Repository;

public class BaseRepository<TEntity> : BaseContext, IBaseRepository<TEntity>, IDisposable where TEntity : class
{
    private readonly EntityDataContext entityUnitOfWork;

    public BaseRepository(IConfiguration configuration)
    {
        entityUnitOfWork = NovaConexao(configuration);
    }

    public EntityDataContext UnitOfWork
    {
        get
        {
            return entityUnitOfWork;
        }
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                UnitOfWork.Dispose();
            }
        }
        disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IQueryable<TEntity> Queryable()
    {
        return UnitOfWork.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    public async Task SaveChangesAsync()
    {
        await UnitOfWork.SaveChangesAsync();
    }
}
