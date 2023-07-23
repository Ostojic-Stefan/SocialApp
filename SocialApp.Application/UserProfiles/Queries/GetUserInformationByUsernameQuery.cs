using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.UserProfiles.Queries;

public class GetUserInformationByUsernameQuery : IRequest<Result<UserInformationResponse>>
{
    public required string Username { get; set; }
}
