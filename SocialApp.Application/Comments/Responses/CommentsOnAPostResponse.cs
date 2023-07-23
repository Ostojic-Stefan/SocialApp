namespace SocialApp.Application.Comments.Responses;

public class CommentsOnAPostResponse
{
    public required Guid PostId { get; set; }
    public required IReadOnlyList<CommentResponse> Comments { get; set; }
}
