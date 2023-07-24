using SocialApp.Domain;

namespace SocialApp.Api.Requests.Likes;

public class AddLikeRequest
{
    public required LikeReaction LikeReaction { get; set; }
}
