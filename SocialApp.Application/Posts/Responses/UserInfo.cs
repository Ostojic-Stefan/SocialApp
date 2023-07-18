namespace SocialApp.Application.Posts.Responses;

public class UserInfo
{
    public required string Username { get; set; }
    public string? Biography { get; set; }
    public string? AvatarUrl { get; set; }
}
