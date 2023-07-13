using System.Linq.Expressions;

namespace EfCoreHelpers;

public interface IRepository<T>
{
    IQueryable<T> Query();
    IQueryable<T> QueryById(Guid id);
    IQueryable<T> QueryByIds(IList<Guid> ids);
    Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
