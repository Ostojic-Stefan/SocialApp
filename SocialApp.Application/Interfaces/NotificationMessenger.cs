using SocialApp.Domain;
using System.Threading.Channels;

namespace SocialApp.Application.Interfaces;

public enum NotificationType
{
    Comment, Like
}

public abstract class NotificationMessage
{
    public required Guid SenderUserId { get; set; }
    public required Post Post { get; set; }
    public abstract NotificationType Type { get; }
}

public class CommentNotificationMessage : NotificationMessage
{
    public required Comment Comment { get; set; }

    public override NotificationType Type => NotificationType.Comment;
}


public class LikeNotificationMessage : NotificationMessage
{
    public required PostLike Like { get; set; }

    public override NotificationType Type => NotificationType.Like;
}


public interface INotificationMessenger
{
    ValueTask AddAsync(NotificationMessage notification, CancellationToken cancellationToken);
    ValueTask<NotificationMessage> GetNotificationAsync(CancellationToken cancellationToken);
    ValueTask<bool> WaitForData(CancellationToken cancellationToken);
}

public class NotificationMessenger : INotificationMessenger
{
    private readonly Channel<NotificationMessage> _channel;

    public NotificationMessenger()
    {
        _channel = Channel.CreateUnbounded<NotificationMessage>();
    }

    public async ValueTask AddAsync(NotificationMessage notification, CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(notification, cancellationToken);
    }

    public ValueTask<NotificationMessage> GetNotificationAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAsync(cancellationToken);
    }

    public ValueTask<bool> WaitForData(CancellationToken cancellationToken)
    {
        return _channel.Reader.WaitToReadAsync(cancellationToken);
    }
}
