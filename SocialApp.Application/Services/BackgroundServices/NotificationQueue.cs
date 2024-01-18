using System.Collections.Concurrent;

namespace SocialApp.Application.Services.BackgroundService;

public class NotificationQueue : INotificationQueue
{
    private readonly ConcurrentQueue<QueueData> _queue = new();

    public void AddNotification(QueueData notification)
    {
        _queue.Enqueue(notification);
    }

    public QueueData? RemoveNotification()
    {
        return _queue.TryDequeue(out var notification) ? notification : null;
    }
}
