using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Extensions;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetUserFriendsPostQueryHandler
    : DataContextRequestHandler<GetUserFriendsPostsQuery, Result<IReadOnlyList<PostResponse>>>
{
    public GetUserFriendsPostQueryHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<IReadOnlyList<PostResponse>>> Handle(GetUserFriendsPostsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<PostResponse>>();
        try
        {
            var userRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var friendIds = await userRepo
                .QueryById(request.CurrentUserId)
                .Select(u => u.Friends)
                .SelectMany(u => u.Select(friend => friend.Id))
                .ToListAsync(cancellationToken);
            var posts = await postRepo
                .Query()
                .Where(post => friendIds.Contains(post.UserProfileId))
                .Include(post => post.UserProfile.ProfileImage)
                .Include(post => post.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Select(post => post.MapToPostReponse(request.CurrentUserId))
                .ToListAsync(cancellationToken);
            result.Data = posts;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
            throw;
        }
        return result;
    }
}
