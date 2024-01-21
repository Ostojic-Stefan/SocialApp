using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Extensions;
using SocialApp.Application.UserProfiles.Queries;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.QueryHandlers;

internal class GetLikesQueryHandler
    : DataContextRequestHandler<GetUserLikesQuery, Result<LikesForUserResponse>>
{
    private readonly IMapper _mapper;

    public GetLikesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<LikesForUserResponse>> Handle(GetUserLikesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<LikesForUserResponse>();
        try
        {
            var userRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var likeRepo = _unitOfWork.CreateReadOnlyRepository<PostLike>();
            // TODO: fix
            var likes = await likeRepo
                .Query()
                .Where(l => l.UserProfile.Username == request.Username)
                .Include(l => l.UserProfile.ProfileImage)
                .Select(l => new PostLikeResponse
                {
                    Id = l.Id,
                    LikeReaction = l.LikeReaction,
                    UserInformation = l.UserProfile.MapToUserInfo()
                })
                .ToListAsync(cancellationToken);

            var userInfo = await userRepo
                .Query()
                .Include(u => u.ProfileImage)
                .Select(u => u.MapToUserInfo())
                .SingleAsync(u => u.Username == request.Username, cancellationToken);
            result.Data = new LikesForUserResponse
            {
                Likes = likes,
                UserInfo = userInfo
            };
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}    

public class LikesForUserResponse
{
    public required UserInfo UserInfo { get; set; }
    public required IReadOnlyList<PostLikeResponse> Likes { get; set; }
}
