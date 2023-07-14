using EfCoreHelpers;
using SocialApp.Domain.Exceptions;
using SocialApp.Domain.Validators;

namespace SocialApp.Domain;

public class Comment : BaseEntity
{
    private Comment() { }

    //public int Id { get; private set; }
    public string Contents { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Relationships
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public Guid PostId { get; private set; }
    public Post Post { get; private set; }

    public static Comment CreateComment(string contents, Guid userProfileId, Guid postId)
    {
        var newComment = new Comment
        {
            Contents = contents,
            UserProfileId = userProfileId,
            PostId = postId
        };

        var commentValidator = new CommentValidator();
        var validationResult = commentValidator.Validate(newComment);
        if (!validationResult.IsValid)
        {
            throw new ModelInvalidException("invalid comment parameters",
                validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray());
        }
        return newComment;
    }
}