namespace SocialApp.Application.UserProfiles.Responses;

public class UserInformationResponse
{
    public required Guid UserProfileId { get; set; }
    public required string Username { get; set; }
    public string? Biography { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
