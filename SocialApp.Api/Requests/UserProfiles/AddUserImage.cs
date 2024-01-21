using System.ComponentModel.DataAnnotations;

namespace SocialApp.Api.Requests.UserProfiles;

public class AddUserImage
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "avatar url should contain some data")]
    [StringLength(240, ErrorMessage = "avatar url should not have more than 240 characters")]
    public required string AvatarUrl { get; set; }
}
