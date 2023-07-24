using MediatR;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Likes.Commands;

public class DeleteLikeCommand : IRequest<Result<PostLikeResponse>>
{
    public required Guid LikeId { get; set; }
    public required Guid UserProfileId { get; set; }
}
