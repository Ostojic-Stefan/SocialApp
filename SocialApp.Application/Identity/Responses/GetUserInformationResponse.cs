using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Identity.Responses;

public class FriendRequestResponse
{
    public required Guid RequesterId { get; set; }
    public string? RequesterAvatarUrl { get; set; }
    public required string RequesterUsername { get; set; }
    public DateTime RequestTimeSent { get; set; }
}

public class Notifications
{
    public IEnumerable<CommentOnPost>? CommentsOnPost { get; set; }
    public IEnumerable<LikeOnPost>? LikesOnPost { get; set; }
}

public class CommentOnPost
{
    public required Guid CommentId { get; set; }
    public required Guid PostId { get; set; }
    public required UserInfo UserInformation { get; set; }
    public required string ContentsReduced { get; set; }
    public required PostResponse PostResponse { get; set; }
    //public string? CommenterAvatarUrl { get; set; }
    //public required string CommenterUsername { get; set; }
}

public class LikeOnPost
{
    public required Guid LikeId { get; set; }
    public required Guid PostId { get; set; }
    public required UserInfo UserInformation { get; set; }
    public required LikeReaction LikeReaction { get; set; }
    public required PostResponse PostResponse { get; set; }


    //public string? LikerAvatarUrl { get; set; }
    //public required string LikerUsername { get; set; }
}

public class UserInformation
{
    public required Guid UserId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
    public required string Username { get; set; }
    public string? Biography { get; set; }
    public string? AvatarUrl { get; set; }
}

public class GetUserInformationResponse
{
    public UserInformation? UserInformation { get; set; }
    public Notifications? Notifications { get; set; }
    public IReadOnlyList<FriendRequestResponse>? FriendRequests { get; set; }
}
