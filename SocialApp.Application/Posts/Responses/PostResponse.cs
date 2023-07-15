namespace SocialApp.Application.Posts.Responses;

public class PostResponse
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public string? Contents { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid UserProfileId { get; set; }
}
