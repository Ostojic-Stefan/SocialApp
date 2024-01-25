using MediatR;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Responses;

namespace SocialApp.Application.UserProfiles.Commands;

public class UpdateUserProfileCommand : IRequest<Result<bool>>
{
    public required Guid CurrentUserId { get; set; }
    public required string Username { get; set; }
    public required string Biography { get; set; }
}
