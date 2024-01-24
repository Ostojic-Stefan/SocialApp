using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Extensions;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetPostLikesQueryHandler
    : DataContextRequestHandler<GetPostLikesQuery, Result<GetLikesForAPostResponse>>
{
    private readonly IMapper _mapper;

    public GetPostLikesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
            _mapper = mapper;
    }

    public override async Task<Result<GetLikesForAPostResponse>> Handle(GetPostLikesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<GetLikesForAPostResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
            var post = await postRepo
                .QueryById(request.PostId)
                .Where(p => p.DoneProcessing)
                .Include(p => p.Likes)
                .ThenInclude(p => p.UserProfile.ProfileImage)
                .FirstAsync(cancellationToken);

            // TODO: to do
            //if (post.UserProfileId == request.CurrentUser)
            //{
            //    foreach (var like in post.Likes)
            //        like.SetLikeAsSeen();
            //}

            result.Data = new GetLikesForAPostResponse
            {
                PostId = post.Id,
                Likes = post.Likes.Select(l => new PostLikeResponse
                { 
                    Id = l.Id,
                    LikeReaction = l.LikeReaction,
                    UserInfo = l.UserProfile.MapToUserInfo()
                }).ToList()
            };

            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}