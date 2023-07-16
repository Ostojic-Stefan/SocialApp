using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.Services;

namespace SocialApp.Application.Posts.Queries;

public class GetAllPostsQuery 
    : IRequest<Result<PagedList<PostResponse>>>
{
    public required int PageSize { get; set; }
    public required int PageNumber { get; set; }
}
