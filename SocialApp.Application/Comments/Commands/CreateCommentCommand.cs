using MediatR;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Comments.Commands;

public class CreateCommentCommand : IRequest<Result<CommentResponse>>
{
    public required Guid UserProfileId { get; set; }
    public required Guid PostId { get; set; }
    public required string Contents { get; set; }
}
