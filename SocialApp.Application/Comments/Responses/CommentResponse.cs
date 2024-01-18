using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.Comments.Responses;

public class CommentResponse
{
    public required Guid Id { get; set; }
    public required string Contents { get; set; }
    public required UserInfo UserInfo { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}
