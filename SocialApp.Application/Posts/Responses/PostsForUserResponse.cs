using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.Posts.Responses;

public class PostsForUserResponse
{
    public required UserInfo UserInfo { get; set; }
    public required IReadOnlyList<PostResponse> Posts { get; set; }
}
