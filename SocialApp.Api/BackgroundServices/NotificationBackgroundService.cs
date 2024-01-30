using EfCoreHelpers;
using SocialApp.Api.SignalR.Notification;
using SocialApp.Application.Interfaces;
using SocialApp.Domain;

namespace SocialApp.Api.BackgroundServices;

public class NotificationBackgroundService : BackgroundService
{
    private readonly INotificationMessenger _notificationMessenger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public NotificationBackgroundService(
        INotificationMessenger notificationMessenger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _notificationMessenger = notificationMessenger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _notificationMessenger.WaitForData(stoppingToken))
        {
            var message = await _notificationMessenger.GetNotificationAsync(stoppingToken);

            if (message.SenderUserId != message.Post.UserProfileId)
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var notificationHubService = scope.ServiceProvider.GetRequiredService<NotificationHubService>();

                switch (message.Type)
                {
                    case NotificationType.Comment:
                        await SaveCommentNotification((CommentNotificationMessage)message, unitOfWork, stoppingToken);
                        await SendCommentNotification((CommentNotificationMessage)message, notificationHubService);
                        break;
                    case NotificationType.Like:
                        await SaveLikeNotification((LikeNotificationMessage)message, unitOfWork, stoppingToken);
                        await SendLikeNotification((LikeNotificationMessage)message, notificationHubService);
                        break;
                }
            }
        }
    }

    private static async Task SendCommentNotification(CommentNotificationMessage message,
        NotificationHubService notificationHubService)
    {
        await notificationHubService.NotifyForComment(
            message.Post.UserProfile.Id.ToString(),
            message.Comment,
            message.Post);
    }

    private static async Task SendLikeNotification(LikeNotificationMessage message,
        NotificationHubService notificationHubService)
    {
        await notificationHubService.NotifyForLike(
                message.Post.UserProfile.Id.ToString(),
                message.Like,
                message.Post);
    }

    private static async Task SaveCommentNotification(
        CommentNotificationMessage message,
        IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var repo = unitOfWork.CreateReadWriteRepository<Notification>();
        var notification = Notification.CreateForComment(
            message.SenderUserId,
            message.Post.Id,
            message.Post.UserProfileId,
            message.Comment.Id);
        repo.Add(notification);
        await unitOfWork.SaveAsync(cancellationToken);
    }

    private static async Task SaveLikeNotification(LikeNotificationMessage message,
        IUnitOfWork unitOfWork, CancellationToken cancellationToken)
    {
        var repo = unitOfWork.CreateReadWriteRepository<Notification>();
        var notification = Notification.CreateForLike(
            message.SenderUserId,
            message.Post.Id,
            message.Post.UserProfileId,
            message.Like.Id);
        repo.Add(notification);
        await unitOfWork.SaveAsync(cancellationToken);
    }
}
