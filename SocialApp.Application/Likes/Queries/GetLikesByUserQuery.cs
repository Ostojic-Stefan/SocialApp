using MediatR;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Likes.Queries;

public class GetLikesByUserQuery : IRequest<Result<IReadOnlyList<PostLikeResponse>>>
{
    public required Guid CurrentUserId { get; set; }
    public required string Username { get; set; }
}
