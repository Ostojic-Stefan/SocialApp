using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.Identity.Responses;

public class FriendRequestResponse
{
    public required UserInfo RequesterUser { get; set; }
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
