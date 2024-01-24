using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Responses;

public class PostLikeResponse
{
    public required Guid Id { get; set; }
    public required LikeReaction LikeReaction { get; set; }
    public required UserInfo UserInfo { get; set; }
}