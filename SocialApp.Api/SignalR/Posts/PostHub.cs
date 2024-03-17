using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace SocialApp.Api.SignalR.Posts;

[Authorize]
public class PostHub : Hub
{
    private readonly PostHubCache _postHubCache;

    public PostHub(PostHubCache postHubCache)
    {
        _postHubCache = postHubCache;
    }

    public async Task StartTyping(Guid postId)
    { 
        var userId = GetUserId(Context.User);
        var username = await _postHubCache.GetUsername(postId, userId);
        await Clients.OthersInGroup(postId.ToString()).SendAsync("ReceiveTyping", username);
    }

    public async Task SendComment(Guid postId, string message)
    {
        await Clients.Group(postId.ToString()).SendAsync("ReceiveComment", message);
    }

    public async Task JoinPost(Guid postId)
    {
        var userId = GetUserId(Context.User);
        await _postHubCache.AddUser(postId, userId);
        await Groups.AddToGroupAsync(Context.ConnectionId, postId.ToString());
    }

    public async Task LeavePost(Guid postId)
    {
        var userId = GetUserId(Context.User);
        _postHubCache.RemoveUser(postId, userId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, postId.ToString());
    }

    private static Guid GetUserId(ClaimsPrincipal? claimsPrincipal)
    {
        var str = claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserProfileId")?.Value;
        return Guid.Parse(str!);
    }
}
