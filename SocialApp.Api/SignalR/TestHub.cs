using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SocialApp.Api.SignalR;

[Authorize]
public class TestHub : Hub
{
    public override Task OnConnectedAsync()
    {
        var user = Context.User.FindFirst("UserProfileId").Value;

        return base.OnConnectedAsync();
    }
    public async Task NewMessage(string user, string message)
    {
        await Clients.All.SendAsync("messageReceived", user, message);
    }
}
