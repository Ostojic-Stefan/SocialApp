using MediatR;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Identity.Commands;

public class RegisterCommand : IRequest<Result<IdentityResponse>>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Biography { get; set; }
    public string? AvatarUrl { get; set; }
}
