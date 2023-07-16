using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Commands;

public class DeletePostCommand : IRequest<Result<PostResponse>>
{
    public required Guid PostId { get; set; }
    public required Guid UserProfileId { get; set; }
}
