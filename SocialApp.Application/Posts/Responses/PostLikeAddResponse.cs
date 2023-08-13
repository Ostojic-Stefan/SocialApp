namespace SocialApp.Application.Posts.Responses;

public class PostLikeAddResponse
{
    public required Guid LikeId { get; set; }
    public required Guid PostId { get; set; }
}
