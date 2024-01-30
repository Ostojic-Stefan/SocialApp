using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SocialApp.Api.SignalR;

public class UserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirstValue("UserProfileId");
    }
}
