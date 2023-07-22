using SocialApp.Api.Validations;

namespace SocialApp.Api.Requests.UserProfiles;

public class UploadUserProfileImageRequest
{
    [MaxFileSize(20 * 1024 * 1204)]
    [AllowedExtensions(".jpg", ".jpeg", ".png")]
    public required IFormFile Img { get; set; }
}
