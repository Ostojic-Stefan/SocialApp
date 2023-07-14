using Microsoft.EntityFrameworkCore;

namespace EfCoreHelpers;
internal class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
{
    private readonly DbContext context;

    public ReadOnlyRepository(DbContext context)
    {
        this.context = context;
    }

    public IQueryable<T> Query()
    {
        return context.Set<T>()
            .AsNoTracking();
    }

    public IQueryable<T> QueryById(Guid id)
    {
        return Query().Where(m => m.Id == id);
    }

    public IQueryable<T> QueryByIds(IList<Guid> ids)
    {
        return Query().Where(m => ids.Contains(m.Id));
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Query()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }
}
