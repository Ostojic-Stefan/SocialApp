using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.Services;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler
    : DataContextRequestHandler<GetAllPostsQuery, Result<IReadOnlyList<PostResponse>>>
{
    private readonly IMapper _mapper;

    public GetAllPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<PostResponse>>> Handle(GetAllPostsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<PostResponse>>();
        try
        {
            var repo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var likeRepo = _unitOfWork.CreateReadOnlyRepository<PostLike>();

            var posts = await repo
             .Query()
             .Include(p => p.Likes)
             .OrderByDescending(p => p.CreatedAt)
             .Select(p => new PostResponse
             {
                 Id = p.Id,
                 Contents = p.Contents,
                 UserInfo = new UserInfo
                 {
                     UserProfileId = p.UserProfileId,
                     Username = p.UserProfile.Username,
                     AvatarUrl = p.UserProfile.AvatarUrl,
                 },
                 ImageUrl = p.ImageUrl,
                 NumComments = p.Comments.Count(),
                 NumLikes = p.Likes.Count(),
                 LikeInfo = p.Likes.Any(l => l.UserProfileId == request.CurrentUserId)
                     ? new LikeInfo
                     {
                         LikedByCurrentUser = true,
                         LikeId = p.Likes.First(l => l.UserProfileId == request.CurrentUserId).Id,
                     }
                     : null,
                 CreatedAt = p.CreatedAt,
                 UpdatedAt = p.UpdatedAt,
             })
             .ToListAsync(cancellationToken);

            result.Data = posts;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
