using DataAccess;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Domain;
using System.Collections.Generic;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler
    : DataContextRequestHandler<GetAllPostsQuery, Result<IReadOnlyList<Post>>>
{
    public GetAllPostsQueryHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<IReadOnlyList<Post>>> Handle(GetAllPostsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<Post>>();
        var repo = _unitOfWork.CreateReadOnlyRepository<Post>();
        var posts = await repo.GetAllAsync(cancellationToken);
        result.Data = posts;
        return result;
    }
}
