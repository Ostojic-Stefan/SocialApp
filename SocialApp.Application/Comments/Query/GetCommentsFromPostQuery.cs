using MediatR;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Comments.Query;

public class GetCommentsFromPostQuery : IRequest<Result<CommentsOnAPostResponse>>
{
    public required Guid PostId { get; set; }
}
