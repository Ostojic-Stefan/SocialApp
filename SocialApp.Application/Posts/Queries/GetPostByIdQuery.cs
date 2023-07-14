using MediatR;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Queries;

public class GetPostByIdQuery : IRequest<Result<Post>>
{
    public Guid PostId { get; set; }
}
