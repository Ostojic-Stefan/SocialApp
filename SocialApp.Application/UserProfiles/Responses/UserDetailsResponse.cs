namespace SocialApp.Application.UserProfiles.Responses;

// TODO: Add Denied Status
public enum FriendStatus
{
    Friend,
    NotFriend,
    WaitingAcceptance,  // waiting for current user to accept / decline
    WaitingApproval     // current user sent request
}

public class UserDetailsResponse
{
    public required UserInfo UserInfo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public FriendStatus FriendStatus { get; set; }
    public required int NumPosts { get; set; }
    public required int NumFriends { get; set; }
    public required int NumLikes { get; set; }
}
