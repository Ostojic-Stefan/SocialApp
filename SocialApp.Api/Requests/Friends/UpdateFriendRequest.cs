using SocialApp.Domain;

namespace SocialApp.Api.Requests.Friends;

public class UpdateFriendRequest
{
    public FriendRequestUpdateStatus Status { get; set; }    
}