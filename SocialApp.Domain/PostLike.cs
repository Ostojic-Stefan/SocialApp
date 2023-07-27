using EfCoreHelpers;

namespace SocialApp.Domain;

public class PostLike : BaseEntity
{
    public Guid UserProfileId { get; set; }
    public UserProfile UserProfile { get; set; }
    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public LikeReaction LikeReaction { get; set; }

    public bool SeenByPostOwner { get; set; } = false;

    public static PostLike Create(Guid postId, Guid userProfileId, LikeReaction likeReaction)
    {
        // TODO: Validation
        return new PostLike
        {
            PostId = postId,
            UserProfileId = userProfileId,
            LikeReaction = likeReaction
        };
    }

    public void SetLikeAsSeen()
    {
        SeenByPostOwner = true;
    }
}
