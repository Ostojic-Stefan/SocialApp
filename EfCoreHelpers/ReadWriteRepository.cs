using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EfCoreHelpers;
/// <summary>
/// This class is an implementation of the <see cref="IRepository{T}"/> Guiderface for
/// working with entities that implement the <see cref="IEntity"/> Guiderface.
/// </summary>
/// <typeparam name="T">The type of the entities of this repository.</typeparam>
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

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Query()
            .SingleAsync(m => m.Id == id, cancellationToken)
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

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken).ConfigureAwait(false);
        context.Set<T>().Remove(entity);
    }

    public async Task RemoveAsync(IEnumerable<Guid> entityIds, CancellationToken cancellationToken)
    {
        List<Task> entitiesToRemove = new();
        foreach (Guid entityId in entityIds)
            entitiesToRemove.Add(RemoveAsync(entityId, cancellationToken));
        await Task.WhenAll(entitiesToRemove).ConfigureAwait(false);
    }

    public async Task RemoveAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        List<Task> entitiesToRemove = new();
        foreach (T entity in entities)
            entitiesToRemove.Add(RemoveAsync(entity.Id, cancellationToken));
        await Task.WhenAll(entitiesToRemove).ConfigureAwait(false);
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
}
