using SocialApp.Domain;

namespace SocialApp.Application.Likes.Responses;

public class PostLikeResponse
{
    public Guid UserProfileId { get; set; }
    public Guid PostId { get; set; }
    public LikeReaction LikeReaction { get; set; }
}
