using MediatR;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Comments.Query;

public class GetCommentsForUserQuery : IRequest<Result<IReadOnlyList<CommentResponse>>>
{
    public required string Username;
}