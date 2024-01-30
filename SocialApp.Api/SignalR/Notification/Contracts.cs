using SocialApp.Domain;

namespace SocialApp.Api.SignalR.Notification;

public class CommentNotificationResponse
{
    public required string SenderUsername { get; set; }
    public required string CommentContents { get; set; }
    public required Guid PostId { get; set; }
    public required string PostTitle { get; set; }
}

public class LikeNotificationResponse
{
    public required string SenderUsername { get; set; }
    public required LikeReaction LikeReaction { get; set; }
    public required Guid PostId { get; set; }
    public required string PostTitle { get; set; }
}
