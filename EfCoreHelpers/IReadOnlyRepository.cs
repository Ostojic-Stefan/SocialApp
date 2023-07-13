namespace EfCoreHelpers;

public interface IReadOnlyRepository<T> : IRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken);
}