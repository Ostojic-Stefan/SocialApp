using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Identity.Queries;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;
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
                .Select(n => new NotificationResponse
                {
                    NotificationType = n.Comment != null ? "Comment" : "Like",
                    Comment = n.Comment != null ? new CommentResponse
                    {
                        Contents = n.Comment.Contents,
                        Id = n.Comment.Id,
                        UserInfo = new UserInfo
                        {
                            Username = n.Comment.UserProfile.Username,
                            AvatarUrl = n.Comment.UserProfile.AvatarUrl,
                            Biography = n.Comment.UserProfile.Biography,
                            UserProfileId = n.Comment.UserProfile.Id
                        },
                        CreatedAt = n.Comment.CreatedAt,
                        UpdatedAt = n.Comment.UpdatedAt,
                    } : null,
                    Like = n.Like != null ? new PostLikeResponse
                    {
                        Id = n.Like.Id,
                        LikeReaction = n.Like.LikeReaction,
                        UserInformation = new UserInfo
                        {
                            Username = n.Like.UserProfile.Username,
                            AvatarUrl = n.Like.UserProfile.AvatarUrl,
                            Biography = n.Like.UserProfile.Biography,
                            UserProfileId = n.Like.UserProfile.Id
                        }
                    } : null
                }).ToListAsync(cancellationToken);

            var userProfile = await userProfileRepo
                .QueryById(request.UserProfileId)
                .ProjectTo<UserInfo>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            var final = new GetUserInformationResponse
            {
                FriendRequests = userFriendRequestsResponse,
                UserInformation = userProfile,
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
