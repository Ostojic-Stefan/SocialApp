namespace SocialApp.Application.UserProfiles.Responses;

// used for when querying for user profile page
public class UserDetailsResponse
{
    public required UserInfo UserInfo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsFriend { get; set; }
}
