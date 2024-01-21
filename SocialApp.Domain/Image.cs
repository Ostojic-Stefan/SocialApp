using EfCoreHelpers;

namespace SocialApp.Domain;

public enum ImageType
{
    User, Post
};

public class Image : BaseEntity
{
    public required string OriginalImagePath { get; set; }
    public required string ThumbnailImagePath { get; set; }
    public required string FullscreenImagePath { get; set; }

    // Navigation properties for the polymorphic association
    public UserProfile? User { get; set; }
    public Guid? UserProfileId { get; set; }

    public Guid? PostId { get; set; }
    public Post? Post { get; set; }

    public static Image GenerateDefaultAvatar()
    {
        return new Image
        {
            FullscreenImagePath = "http://localhost:5000/static/default-avatar.jpg",
            OriginalImagePath = "http://localhost:5000/static/default-avatar.jpg",
            ThumbnailImagePath = "http://localhost:5000/static/default-avatar.jpg"
        };
    }
}
