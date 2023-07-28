﻿using SocialApp.Application.Likes.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Responses;

public class PostResponse
{
    public Guid Id { get; set; }
    public string? ImageUrl { get; set; }
    public string? Contents { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required UserInfo UserInfo { get; set; }
    public required int NumLikes { get; set; }
    //public required IReadOnlyList<GetLikesForAPostResponse> PostLikes { get; set; }
}
