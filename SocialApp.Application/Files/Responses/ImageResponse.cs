namespace SocialApp.Application.Files.Responses;

public class ImageResponse
{
    public required Guid ImageId { get; set; }
    public required string OriginalImagePath { get; set; }
    public required string ThumbnailImagePath { get; set; }
    public required string FullscreenImagePath { get; set; }
}
