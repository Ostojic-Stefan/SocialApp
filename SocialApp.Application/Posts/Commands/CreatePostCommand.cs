﻿using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Commands;

public class CreatePostCommand : IRequest<Result<PostResponse>>
{
    public required string ImageUrl { get; set; }
    public required string Contents { get; set; }
    public required Guid UserProfileId { get; set; }
}
