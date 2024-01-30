using SocialApp.Domain;
using System.Threading.Channels;

namespace SocialApp.Application.Interfaces;

public record NotificationMessage(Guid SenderUserId, Post Post, Comment? Comment, PostLike? Like);

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
