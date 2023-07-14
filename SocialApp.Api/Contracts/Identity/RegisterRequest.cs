namespace SocialApp.Api.Contracts.Identity;

public class RegisterRequest
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Biography { get; set; }
    public string? AvatarUrl { get; set; }

}
