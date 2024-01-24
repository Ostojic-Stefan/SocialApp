using SocialApp.Application.Files.Responses;
using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.Posts.Responses;

public class PostLikeInfo
{
    public required Guid LikeId { get; set; }
    public required bool LikedByCurrentUser { get; set; }
}

public class PostResponse
{
    public Guid Id { get; set; }
    public required IEnumerable<ImageResponse> Images { get; set; }
    public required string Title { get; set; }
    public required string Contents { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required UserInfo UserInfo { get; set; }
    public required PostLikeInfo LikeInfo { get; set; }
    public required int NumLikes { get; set; }
    public required int NumComments { get; set; }
}
