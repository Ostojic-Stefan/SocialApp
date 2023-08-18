namespace SocialApp.Application.UserProfiles.Responses;

public class UserInfo
{
    public Guid UserProfileId { get; set; }
    public required string Username { get; set; }
    public string? AvatarUrl { get; set; }
}
