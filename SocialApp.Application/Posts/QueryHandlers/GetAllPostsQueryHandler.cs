using DataAccess;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Posts.Queries;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler
    : DataContextRequestHandler<GetAllPostsQuery, IReadOnlyList<Post>>
{
    public GetAllPostsQueryHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<IReadOnlyList<Post>> Handle(GetAllPostsQuery request,
        CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.CreateReadOnlyRepository<Post>();
        var posts = await repo.GetAllAsync(cancellationToken);
        return posts;
    }
}
