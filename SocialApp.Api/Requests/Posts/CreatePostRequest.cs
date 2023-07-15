namespace SocialApp.Api.Requests.Posts;

public class CreatePostRequest
{
    public required string ImageUrl { get; set; }
    public required string Contents { get; set; }
    public required string UserProfileId { get; set; }
}
