using SocialApp.Domain;

namespace SocialApp.Application.Services.BackgroundService;

public record QueueData(Guid SenderUserId, Post Post, Comment? Comment, PostLike? Like);

public interface INotificationQueue
{
    void AddNotification(QueueData notification);
    QueueData? RemoveNotification();
}
