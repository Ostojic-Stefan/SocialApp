using SocialApp.Application.Files.Responses;

namespace SocialApp.Application.UserProfiles.Responses;

public class UserInfo
{
    public Guid UserProfileId { get; set; }
    public required string Username { get; set; }
    public required ImageResponse ProfileImage { get; set; }
    public string? Biography { get; set; }
}
