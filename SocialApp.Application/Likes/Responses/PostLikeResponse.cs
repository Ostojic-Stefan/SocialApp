using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Likes.Responses;

public class PostLikeResponse
{
    public required LikeReaction LikeReaction { get; set; }
    public Guid UserProfileId { get; set; }
    public required UserInfo UserInformation { get; set; }
}