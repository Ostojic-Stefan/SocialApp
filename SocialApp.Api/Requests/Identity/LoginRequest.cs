using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Requests.Identity;

public class LoginRequest
{
    [EmailAddress(ErrorMessage = "Invalid Email Address Format")]
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}
