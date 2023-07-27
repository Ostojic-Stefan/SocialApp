using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Identity.Queries;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Identity.QueryHandlers;

internal class GetUserInformationQueryHandler
    : DataContextRequestHandler<GetUserInformationQuery, Result<GetUserInformationResponse>>
{
    private readonly IMapper _mapper;

    public GetUserInformationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<GetUserInformationResponse>> Handle(GetUserInformationQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<GetUserInformationResponse>();
        try
        {
            var userProfileRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();

            // add filter for already accepted or denied friend requsts.
            // add DateWhenSent for the request entity
            var userFriendRequestsResponse = await userProfileRepo
                .QueryById(request.UserProfileId)
                .SelectMany(u => u.ReceivedFriendRequests.Select(fr => new FriendRequestResponse
                {
                    RequesterId = fr.SenderUserId,
                    RequesterUsername = fr.SenderUser.Username,
                    RequesterAvatarUrl = fr.SenderUser.AvatarUrl,
                })).ToListAsync(cancellationToken);

            // add filter for unread comments.
            var commentsOnUsersPosts = await postRepo
                .Query()
                .Where(p => p.UserProfileId == request.UserProfileId)
                .SelectMany(p => p.Comments.Select(c => new CommentOnPost
                {
                    CommenterUsername = c.UserProfile.Username,
                    CommenterAvatarUrl = c.UserProfile.AvatarUrl,
                    CommentId = c.Id,
                    ContentsReduced = c.Contents,
                })).ToListAsync(cancellationToken);

            // add filter for unread likes.
            var likesOnUsersPosts = await postRepo
                .Query()
                .Where(p => p.UserProfileId == request.UserProfileId)
                .SelectMany(p => p.Likes.Select(l => new LikeOnPost
                {
                    LikerUsername = l.UserProfile.Username,
                    LikerAvatarUrl = l.UserProfile.AvatarUrl,
                    LikeId = l.Id,
                    LikeReaction = l.LikeReaction
                })).ToListAsync(cancellationToken);


            var userProfile = await userProfileRepo
                .QueryById(request.UserProfileId)
                .TagWith($"[{nameof(GetUserInformationQueryHandler)}] - Get user profile information")
                .ProjectTo<UserInformation>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var final = new GetUserInformationResponse
            {
                FriendRequests = userFriendRequestsResponse,
                UserInformation = userProfile,
                Notifications = new Notifications
                {
                    CommentsOnPost = commentsOnUsersPosts,
                    LikesOnPost = likesOnUsersPosts
                }
            };

            result.Data = final;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
