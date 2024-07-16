using ExchangeStuff.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExchangeStuff.Repository;

public class DbFactory : IDisposable
{
    private DbContext _dbContext;
    private bool _disposed;
    private readonly Func<ExchangeStuffContext> _instanceFunc;

    public DbFactory(Func<ExchangeStuffContext> dbContextFactory)
    {
        _instanceFunc = dbContextFactory;
    }

    public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

    public void Dispose()
    {
        if (!_disposed && _dbContext != null)
        {
            _disposed = true;
            _dbContext.Dispose();
        }
    }
}