using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Identity.Responses;

// TODO: fix for new user profile image
public class FriendRequestResponse
{
    public required Guid RequesterId { get; set; }
    public string? RequesterAvatarUrl { get; set; }
    public required string RequesterUsername { get; set; }
    public DateTime RequestTimeSent { get; set; }
}

public class NotificationResponse
{
    public required Guid PostId { get; set; }
    public CommentResponse? Comment { get; set; }
    public PostLikeResponse? Like { get; set; }
    public required string NotificationType { get; set; }
}
public class GetUserInformationResponse
{
    public required UserInfo UserInfo { get; set; }
    public required IReadOnlyList<NotificationResponse> Notifications { get; set; }
    public required IReadOnlyList<FriendRequestResponse> FriendRequests { get; set; }
}
