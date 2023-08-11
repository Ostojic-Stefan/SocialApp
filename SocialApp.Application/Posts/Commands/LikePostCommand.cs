using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Commands;

public class LikePostCommand : IRequest<Result<PostResponse>>
{
    public required Guid UserProfileId { get; set; }
    public required Guid PostId { get; set; }
    public required LikeReaction LikeReaction { get; set; }
}
