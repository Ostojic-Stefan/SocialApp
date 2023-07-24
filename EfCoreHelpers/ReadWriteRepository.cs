using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EfCoreHelpers;
internal class ReadWriteRepository<T> : IReadWriteRepository<T> where T : BaseEntity
{

    private readonly DbContext context;

    public ReadWriteRepository(DbContext context)
    {
        this.context = context;
    }

    public IQueryable<T> Query()
    {
        return context.Set<T>();
    }

    public IQueryable<T> QueryById(Guid id)
    {
        return Query().Where(m => m.Id == id);
    }

    public IQueryable<T> QueryByIds(IList<Guid> ids)
    {
        return Query().Where(m => ids.Contains(m.Id));
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public void Add(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
            Add(entity);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task UpdateAsync(IEnumerable<T> entites, CancellationToken cancellationToken)
    {
        List<Task> updatedEntities = new();
        foreach (var entity in entites)
            updatedEntities.Add(UpdateAsync(entity, cancellationToken));
        await Task.WhenAll(updatedEntities).ConfigureAwait(false);
    }

    public async Task<T> RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        context.Set<T>().Remove(entity);
        return entity;
    }

    public async Task<IEnumerable<T>> RemoveAsync(IEnumerable<Guid> entityIds, CancellationToken cancellationToken)
    {
        List<Task<T>> entitiesToRemove = new();
        foreach (Guid entityId in entityIds)
            entitiesToRemove.Add(RemoveAsync(entityId, cancellationToken));
        return await Task.WhenAll(entitiesToRemove).ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> RemoveAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        List<Task<T>> entitiesToRemove = new();
        foreach (T entity in entities)
            entitiesToRemove.Add(RemoveAsync(entity.Id, cancellationToken));
        return await Task.WhenAll(entitiesToRemove).ConfigureAwait(false);
    }

    public async Task RemoveAsync(Expression<Func<T, bool>> entitySelection, CancellationToken cancellationToken)
    {
        var entities = await Query()
            .Where(entitySelection)
            .Select(m => m.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        foreach (Guid entityId in entities)
            await RemoveAsync(entityId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IList<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Query()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public void Remove(T entity)
    {
        context.Remove(entity);
    }
}
