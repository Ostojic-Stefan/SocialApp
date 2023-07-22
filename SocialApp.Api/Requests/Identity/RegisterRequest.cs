using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Requests.Identity;

public class RegisterRequest
{
    [StringLength(30, MinimumLength = 5, ErrorMessage = "Username length must be between 5 and 30 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Username must cointain some value")]
    public required string Username { get; set; }

    [EmailAddress(ErrorMessage = "Invalid Email Address Format")]
    public required string Email { get; set; }
    public required string Password { get; set; }

    [StringLength(200, ErrorMessage = "Biography length must have at most 200 characters")]
    public string? Biography { get; set; }

    [StringLength(200, ErrorMessage = "AvatarUrl length must have at most 200 characters")]
    public string? AvatarUrl { get; set; }

}
