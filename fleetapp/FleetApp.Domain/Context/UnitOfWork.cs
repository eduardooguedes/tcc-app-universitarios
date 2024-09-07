using Microsoft.Extensions.Configuration;

namespace Dashdine.Domain.Context;

public class UnitOfWork : BaseContext, IDisposable
{
    public EntityDataContext _context;

    public UnitOfWork(IConfiguration configuration)
    {
        _context = NovaConexao(configuration);
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}