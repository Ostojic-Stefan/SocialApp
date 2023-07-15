using MediatR;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Identity.Queries;

public class GetUserInformationQuery : IRequest<Result<UserProfile>>
{
    public required Guid UserProfileId { get; set; }
}
