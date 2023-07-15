using MediatR;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Identity.Queries;

public class GetUserInformationQuery : IRequest<Result<GetUserInformationResponse>>
{
    public required Guid UserProfileId { get; set; }
}
