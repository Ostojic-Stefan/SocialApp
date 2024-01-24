using EfCoreHelpers;
using SocialApp.Domain.Exceptions;
using SocialApp.Domain.Validators;

namespace SocialApp.Domain;

public class Post : BaseEntity
{
    private readonly ICollection<Comment> _comments = new List<Comment>();
    private readonly ICollection<PostLike> _likes = new List<PostLike>();
    private readonly ICollection<Image> _images = new List<Image>();

    private Post() 
    {
        _comments = new List<Comment>();
        _likes = new List<PostLike>();
    }

    public required string Title { get; set; }
    public string Contents { get; private set; }
    public required bool DoneProcessing { get; set; } = false;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Relationships
    // TODO: make private set
    public IEnumerable<Image> Images { get; set; } = new List<Image>();

    public Guid UserProfileId { get; private set; }
    public UserProfile UserProfile { get; private set; }
    public IEnumerable<Comment> Comments => _comments;
    public IEnumerable<PostLike> Likes => _likes;

    public static Post CreatePost(string title, string contents, Guid userId)
    {
        var newPost = new Post
        {
            Title = title,
            Contents = contents,
            UserProfileId = userId,
            DoneProcessing = false
        };
        Validate(newPost);
        return newPost;
    }

    public void AddLike(PostLike postLike)
    {
        _likes.Add(postLike);
    }


    public void UpdateContents(string newContents)
    {
        Contents = newContents;
        Validate(this);
    }

    public void Update(string? imageUrl, string? contents)
    {
        // TODO: look into this later
        //if (contents is not null)
        //    Contents = contents;
        //if (imageUrl is not null)
        //    ImageUrl = imageUrl;
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
