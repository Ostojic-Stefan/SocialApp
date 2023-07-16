using MediatR;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Comments.Query;

public class GetCommentsFromPostQuery : IRequest<Result<IReadOnlyList<CommentResponse>>>
{
    public required Guid PostId { get; set; }
}
