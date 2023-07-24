using MediatR;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Likes.Queries;

public class GetLikesForAPostQuery : IRequest<Result<IReadOnlyList<GetLikesForAPostResponse>>>
{
    public required Guid PostId { get; set; }
}
