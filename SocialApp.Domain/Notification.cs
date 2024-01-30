using EfCoreHelpers;
using System.ComponentModel.Design;

namespace SocialApp.Domain;

public class Notification : BaseEntity
{
    private Notification() { }

    public Guid RecipientUserId { get; private set; }
    public UserProfile RecipientUser { get; private set; }

    public Guid SenderUserId { get; private set; }
    public UserProfile SenderUser { get; private set; }

    public Guid PostId { get; private set; }
    public Post Post { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public bool SeenByRecipient { get; set; } = false;

    public Guid? CommentId { get; private set; }
    public Comment? Comment { get; private set; }
    public Guid? LikeId { get; private set; }
    public PostLike? Like { get; private set; }

    public static Notification CreateNotification(Guid senderUserId, Guid recipientUserId, Guid postId, Guid? commentId, Guid? likeId)
    {
        if ((commentId is null && likeId is null) || (commentId is not null && likeId is not null))
        {
            throw new InvalidOperationException("Either commentId or likeId should have a value");
        }
        var notification = new Notification
        {
            SenderUserId = senderUserId,
            RecipientUserId = recipientUserId,
            PostId = postId,
            CreatedAt = DateTime.UtcNow,
        };
        if (commentId is not null)
        {
            notification.CommentId = commentId.Value;
        } 
        else if (likeId is not null)
        {
            notification.LikeId = likeId.Value;
        }

        // TODO: validation
        return notification;
    }

    public static Notification CreateForComment(Guid senderUserId, Guid postId, Guid recipientUserId, Guid commentId)
    {
        return new Notification
        {
            SenderUserId = senderUserId,
            RecipientUserId = recipientUserId,
            CreatedAt = DateTime.UtcNow,
            PostId = postId,
            CommentId = commentId
        };
    }

    public static Notification CreateForLike(Guid senderUserId, Guid postId, Guid recipientUserId, Guid likeId)
    {
        return new Notification
        {
            SenderUserId = senderUserId,
            RecipientUserId = recipientUserId,
            CreatedAt = DateTime.UtcNow,
            PostId = postId,
            LikeId = likeId
        };
    }
}
