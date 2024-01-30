using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SocialApp.Api.SignalR;

public interface INotificationClient
{
    Task ReceiveNotification(string msg);
}

public class NotificationHub : Hub
{
    private static readonly Dictionary<string, List<string>> _connections = new();

    public override Task OnConnectedAsync()
    {
        var identifier = Context.UserIdentifier;
        var claims = Context.User.Claims;
        //var userId = Context.User?.Claims.FirstOrDefault(x => x.Type == "UserProfileId")?.Value;
        //var connectionId = Context.ConnectionId;

        //if (!_connections.ContainsKey(connectionId))
        //{
        //    _connections[userId] = new();
        //}

        //_connections[userId].Add(connectionId);

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // Remove connection mapping when user disconnects
        //var userId = Context.User?.Claims.FirstOrDefault(x => x.Type == "UserProfileId")?.Value;

        //if (_connections.TryGetValue(userId, out List<string>? value))
        //{
        //    value.Remove(Context.ConnectionId);

        //    if (_connections[userId].Count == 0)
        //    {
        //        _connections.Remove(userId);
        //    }
        //}

        return base.OnDisconnectedAsync(exception);
    }
}
