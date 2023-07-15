namespace SocialApp.Application.Identity.Responses;

public class GetUserInformationResponse
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Username { get; set; }
    public string? Biography { get; set; }
    public string? AvatarUrl { get; set; }
}
