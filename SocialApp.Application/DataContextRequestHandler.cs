using EfCoreHelpers;
using MediatR;

namespace SocialApp.Application;

internal abstract class DataContextRequestHandler<TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly IUnitOfWork _unitOfWork;

    public DataContextRequestHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
