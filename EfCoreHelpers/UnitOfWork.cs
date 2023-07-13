using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace EfCoreHelpers;
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction? _transaction = null;
    
    public UnitOfWork(DbContext dbContext)
    {
        _context = dbContext;
    }

    public DbContext Context => _context;

    public async Task<int> SaveAsync(CancellationToken cancellationToken = default, bool commit = true)
    {
        if (Context.Database.CurrentTransaction == null)
            throw new InvalidOperationException("Saving data to database is only allowed using a transaction.");

        var result = await Context
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        if (commit)
            CommitTransaction();

        return result;
    }

    public void StartTransaction()
    {
        // Avoid to create nested transations
        if (Context.Database.CurrentTransaction is null)
            _transaction = Context.Database.BeginTransaction();
    }
    
    private void CommitTransaction()
    {
        if (_transaction is null)
            throw new InvalidOperationException("No currently active transaction");

        _transaction.Commit();
        _transaction.Dispose();
        _transaction = null;
    }
}
