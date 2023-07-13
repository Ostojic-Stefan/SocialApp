using Microsoft.EntityFrameworkCore;

namespace EfCoreHelpers;

public interface IUnitOfWork
{
    DbContext Context { get; }
    void StartTransaction();
    Task<int> SaveAsync(CancellationToken cancellationToken = default, bool commit = true);
}
