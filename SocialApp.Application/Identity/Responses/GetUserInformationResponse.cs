using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Identity.Responses;

public class FriendRequestResponse
{
    public required Guid RequesterId { get; set; }
    public string? RequesterAvatarUrl { get; set; }
    public required string RequesterUsername { get; set; }
    public DateTime RequestTimeSent { get; set; }
}

public class NotificationResponse
{
    public CommentResponse? Comment { get; set; }
    public PostLikeResponse? Like { get; set; }
    public required string NotificationType { get; set; }
}
public class GetUserInformationResponse
{
    public required UserInfo UserInformation { get; set; }
    public required IReadOnlyList<NotificationResponse> Notifications { get; set; }
    public required IReadOnlyList<FriendRequestResponse> FriendRequests { get; set; }
}
