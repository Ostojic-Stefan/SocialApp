using Microsoft.AspNetCore.SignalR;

namespace SocialApp.Api.SignalR;

public class TestHub : Hub
{
    public async Task NewMessage(string user, string message)
    {
        await Clients.All.SendAsync("messageReceived", user, message);
    }
}
