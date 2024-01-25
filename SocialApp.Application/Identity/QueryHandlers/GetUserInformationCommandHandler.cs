using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Identity.Queries;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Extensions;
using SocialApp.Application.UserProfiles.Responses;
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
            var notificationRepo = _unitOfWork.CreateReadOnlyRepository<Notification>();

            var userFriendRequestsResponse = await userProfileRepo
                .QueryById(request.UserProfileId)
                .SelectMany(u => u.ReceivedFriendRequests)
                .Where(fr => fr.Status == FriendRequestStatus.Pending)
                .ProjectTo<FriendRequestResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var notifications = await notificationRepo.Query()
                .Where(n => n.RecipientUser.Id == request.UserProfileId && !n.SeenByRecipient)
                .AsSplitQuery()
                .Include(n => n.Comment.UserProfile.ProfileImage)
                .Include(n => n.Like.UserProfile.ProfileImage)
                .Select(n => new NotificationResponse
                {
                    PostId = n.PostId,
                    NotificationType = n.Comment != null ? "Comment" : "Like",
                    Comment = n.Comment != null ? new CommentResponse
                    {
                        Contents = n.Comment.Contents,
                        Id = n.Comment.Id,
                        UserInfo = n.Comment.UserProfile.MapToUserInfo(),
                        CreatedAt = n.Comment.CreatedAt,
                        UpdatedAt = n.Comment.UpdatedAt,
                    } : null,
                    Like = n.Like != null ? new PostLikeResponse
                    {
                        Id = n.Like.Id,
                        LikeReaction = n.Like.LikeReaction,
                        UserInfo = n.Like.UserProfile.MapToUserInfo(),
                    } : null
                }).ToListAsync(cancellationToken);

            var userProfile = await userProfileRepo
                .QueryById(request.UserProfileId)
                .Include(user => user.ProfileImage)
                .Select(user => user.MapToUserInfo())
                //.ProjectTo<UserInfo>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var final = new GetUserInformationResponse
            {
                FriendRequests = userFriendRequestsResponse,
                UserInfo = userProfile,
                Notifications = notifications
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
