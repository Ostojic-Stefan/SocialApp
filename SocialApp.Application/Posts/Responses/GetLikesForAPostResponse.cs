namespace SocialApp.Application.Posts.Responses;

public class GetLikesForAPostResponse
{
    public required Guid PostId { get; set; }
    public required IReadOnlyList<PostLikeResponse> LikeInfo { get; set; }
}
