using MediatR;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Likes.Commands;

public class AddLikeToPostCommand : IRequest<Result<PostLikeResponse>>
{
    public required Guid UserProfileId { get; set; }
    public required Guid PostId { get; set; }
    public required LikeReaction LikeReaction { get; set; }
}
