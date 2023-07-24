using SocialApp.Domain;

namespace SocialApp.Application.Likes.Responses;

public class LikeUserInfo
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public string? AvatarUrl { get; set; }
}

public class GetLikesForAPostResponse
{
    public required Guid Id { get; set; }
    public required Guid PostId { get; set; }
    public required LikeReaction LikeReaction { get; set; }
    public required LikeUserInfo UserInformation { get; set; }
}
