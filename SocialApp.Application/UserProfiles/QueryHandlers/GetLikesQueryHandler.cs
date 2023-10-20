using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
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
            var ttt = await userRepo
                .QueryById(request.UserId)
                .Select(u => u.Likes)
                .ProjectTo<LikesForUserResponse>(_mapper.ConfigurationProvider)
                .FirstAsync(cancellationToken);
                // TODO: FIX
            result.Data = ttt;
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
    public required IReadOnlyList<PostLikeForUserResponse> Likes { get; set; }
}

public class PostLikeForUserResponse
{
    public required LikeReaction LikeReaction { get; set; }
    public required PostForLikeUserResponse PostInfo { get; set; }
}

public class PostForLikeUserResponse
{
    public required Guid PostId { get; set; }
    public string ImageUrl { get; set; }
    public string Contents { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required UserInfo UserInfo { get; set; }
}
