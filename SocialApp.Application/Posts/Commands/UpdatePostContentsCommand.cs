using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;

namespace SocialApp.Application.Posts.Commands;

public class UpdatePostContentsCommand : IRequest<Result<PostResponse>>
{
    public required Guid PostId { get; set; }
    public required Guid UserProfileId { get; set; }
    public required string Contents { get; set; }
}
