namespace SocialApp.Application.UserProfiles.Responses;

public class FriendResponse
{
    public required Guid UserProfileId { get; set; }
    public required string AvatarUrl { get; set; }
    public required string Username { get; set; }
}
