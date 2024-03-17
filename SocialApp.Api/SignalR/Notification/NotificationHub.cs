using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SocialApp.Api.SignalR.Notification;

public interface INotificationClient
{
    Task ReceiveCommentNotification(CommentNotificationResponse data);
    Task ReceiveLikeNotification(LikeNotificationResponse data);
}

[Authorize]
public class NotificationHub : Hub<INotificationClient>
{
    private static readonly Dictionary<string, List<string>> _connections = new();

    public override Task OnConnectedAsync()
    {
        var userId = Context.User?.Claims.FirstOrDefault(x => x.Type == "UserProfileId")?.Value;
        var connectionId = Context.ConnectionId;

        if (!_connections.ContainsKey(connectionId))
        {
            _connections[userId] = new();
        }

        _connections[userId].Add(connectionId);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.Claims.FirstOrDefault(x => x.Type == "UserProfileId")?.Value;

        if (_connections.TryGetValue(userId, out List<string>? value))
        {
            value.Remove(Context.ConnectionId);

            if (_connections[userId].Count == 0)
            {
                _connections.Remove(userId);
            }
        }

        return base.OnDisconnectedAsync(exception);
    }
}
