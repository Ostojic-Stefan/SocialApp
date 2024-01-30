using Microsoft.AspNetCore.SignalR;
using SocialApp.Domain;

namespace SocialApp.Api.SignalR.Notification;

public class NotificationHubService
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

    public NotificationHubService(IHubContext<NotificationHub, INotificationClient> hubContext)
	{
        _hubContext = hubContext;
    }

    public async Task NotifyForComment(string recipientUserId, Comment comment, Post post)
    {
        var response = new CommentNotificationResponse
        {
            CommentContents = comment.Contents,
            PostId = post.Id,
            PostTitle = post.Title,
            SenderUsername = comment.UserProfile.Username
        };
        await _hubContext.Clients.User(recipientUserId)
            .ReceiveCommentNotification(response);
    }

    public async Task NotifyForLike(string recipientUserId, PostLike like, Post post)
    {
        var response = new LikeNotificationResponse
        {
            LikeReaction = like.LikeReaction,
            PostId = post.Id,
            PostTitle = post.Title,
            SenderUsername = like.UserProfile.Username
        };
        await _hubContext.Clients.User(recipientUserId)
            .ReceiveLikeNotification(response);
    }
}
