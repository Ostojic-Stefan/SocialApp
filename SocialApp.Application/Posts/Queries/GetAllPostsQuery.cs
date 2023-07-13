using MediatR;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.Queries;

public class GetAllPostsQuery : IRequest<IReadOnlyList<Post>>
{
}
