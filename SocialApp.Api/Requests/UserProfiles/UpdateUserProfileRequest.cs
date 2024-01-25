namespace SocialApp.Api.Requests.UserProfiles;

public class UpdateUserProfileRequest
{
    public required string Username { get; set; }
    public required string Biography { get; set; }
}
