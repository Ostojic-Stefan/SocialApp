using System.Linq.Expressions;

namespace EfCoreHelpers;
public interface IReadWriteRepository<T> : IRepository<T> where T : BaseEntity
{
    void Add(IEnumerable<T> entities);
    void Add(T entity);
    Task<IList<T>> GetAllAsync(CancellationToken cancellationToken);
    void Remove(T entity);
    Task RemoveAsync(Expression<Func<T, bool>> entitySelection, CancellationToken cancellationToken);
    Task<T> RemoveAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> RemoveAsync(IEnumerable<Guid> entityIds, CancellationToken cancellationToken);
    Task<IEnumerable<T>> RemoveAsync(IEnumerable<T> entities, CancellationToken cancellationToken);
    Task UpdateAsync(IEnumerable<T> entites, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken);
}