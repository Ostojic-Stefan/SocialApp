﻿using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.Posts.Responses;

public class PostData
{
    public Guid Id { get; set; }
    public required string ImageUrl { get; set; }
    public required string Contents { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public PostLikeInfo? LikeInfo { get; set; }
    public required int NumLikes { get; set; }
    public required int NumComments { get; set; }
};

public class PostsForUserResponse
{
    public required UserInfo UserInfo { get; set; }
    public required IReadOnlyList<PostData> Posts { get; set; }
}
