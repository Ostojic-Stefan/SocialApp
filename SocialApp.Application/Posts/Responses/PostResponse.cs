﻿namespace SocialApp.Application.Posts.Responses;

public class LikeInfo
{
    public required Guid LikeId { get; set; }
    public required bool LikedByCurrentUser { get; set; }
}

public class PostResponse
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public string? Contents { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required UserInfo UserInfo { get; set; }
    public LikeInfo? LikeInfo { get; set; }
    public required int NumLikes { get; set; }
    public required int NumComments { get; set; }
}
