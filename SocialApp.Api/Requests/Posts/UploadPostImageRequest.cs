using SocialApp.Api.Validations;

namespace SocialApp.Api.Requests.Posts;

public class UploadPostImageRequest
{
    [MaxFileSize(20 * 1024 * 1204)]
    [AllowedExtensions(".jpg", ".jpeg", ".png")]
    public required IFormFile Img { get; set; }
}
