namespace SocialApp.Api.Contracts.Identity;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
