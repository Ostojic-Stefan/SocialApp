using EfCoreHelpers;
using SocialApp.Domain.Exceptions;
using SocialApp.Domain.Validators;

namespace SocialApp.Domain;

public class Post : BaseEntity
{
    private readonly ICollection<Comment> _comments;
    private readonly ICollection<PostLike> _likes;

    private Post() 
    {
        _comments = new List<Comment>();
        _likes = new List<PostLike>();
    }

    public string ImageUrl { get; private set; }
    public string Contents { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Relationships
    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public IEnumerable<Comment>? Comments => _comments;
    public IEnumerable<PostLike>? Likes => _likes;

    public static Post CreatePost(string imageUrl, string contents, Guid userId)
    {
        var newPost = new Post
        {
            ImageUrl = imageUrl,
            Contents = contents,
            UserProfileId = userId
        };
        Validate(newPost);
        return newPost;
    }

    public void AddLike(PostLike postLike)
    {
        _likes.Add(postLike);
    }

    public void UpdateImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl;
        Validate(this);
    }

    public void UpdateContents(string newContents)
    {
        Contents = newContents;
        Validate(this);
    }

    public void UpdateImage(string newImageUrl)
    {
        ImageUrl = newImageUrl;
        Validate(this);
    }

    public void Update(string? imageUrl, string? contents)
    {
        if (contents is not null)
            Contents = contents;
        if (imageUrl is not null)
            ImageUrl = imageUrl;
        Validate(this);
    }

    private static void Validate(Post post)
    {
        var validator = new PostValidator();
        var validationResult = validator.Validate(post);
        if (!validationResult.IsValid)
        {
            throw new ModelInvalidException("invalid post parameters",
                validationResult.Errors.Select(vf => vf.ErrorMessage).ToArray());
        }
    }
}
