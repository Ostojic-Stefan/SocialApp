using EfCoreHelpers;

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
        // TODO: Add Validation

        return new Post
        {
            ImageUrl = imageUrl,
            Contents = contents,
            UserProfileId = userId
        };
    }

    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }
}
