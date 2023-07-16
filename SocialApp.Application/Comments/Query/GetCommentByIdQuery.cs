using MediatR;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Comments.Query;

public class GetCommentByIdQuery : IRequest<Result<CommentResponse>>
{
    public required Guid CommentId { get; set; }
}
