using EfCoreHelpers;
using SocialApp.Domain.Exceptions;
using SocialApp.Domain.Validators;

namespace SocialApp.Domain;

public class Post : BaseEntity
{
    private readonly ICollection<Comment> _comments;

    private Post() 
    {
        _comments = new List<Comment>();
    }

    //public Guid Id { get; private set; }
    public string ImageUrl { get; private set; }
    public string Contents { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Relationships
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public IEnumerable<Comment>? Comments => _comments;

    public static Post CreatePost(string imageUrl, string contents, Guid userId)
    {
        var newPost = new Post
        {
            ImageUrl = imageUrl,
            Contents = contents,
            UserProfileId = userId
        };

        var validator = new PostValidator();
        var validationResult = validator.Validate(newPost);
        if (!validationResult.IsValid)
        {
            throw new ModelInvalidException("invalid post parameters",
                validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray());
        }
        return newPost;
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }
}
