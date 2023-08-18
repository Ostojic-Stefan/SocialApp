using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Commands;
public class DeletePostLikeCommand : IRequest<Result<PostLikeDeleteResponse>>
{
    public required Guid LikeId { get; set; }
    public required Guid UserProfileId { get; set; }
}