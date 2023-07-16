namespace SocialApp.Api.Requests.Comments;

public class CreateComment
{
    public required Guid PostId { get; set; }
    public required string Contents { get; set; }
}
