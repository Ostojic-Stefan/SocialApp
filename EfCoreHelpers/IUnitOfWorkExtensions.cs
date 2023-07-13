namespace EfCoreHelpers;

public static class IUnitOfWorkExtensions
{
    
    public static IReadOnlyRepository<T> CreateReadOnlyRepository<T>(this IUnitOfWork unitOfWork)
        where T : BaseEntity
    {
        return new ReadOnlyRepository<T>(unitOfWork.Context);
    }

    public static IReadWriteRepository<T> CreateReadWriteRepository<T>(this IUnitOfWork unitOfWork)
        where T : BaseEntity
    {
        unitOfWork.StartTransaction();
        return new ReadWriteRepository<T>(unitOfWork.Context);
    }
}
