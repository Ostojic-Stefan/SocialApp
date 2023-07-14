using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.Identity.Commands;

public class RegisterCommand : IRequest<Result<string>>
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? Biography { get; set; }
    public string? AvatarUrl { get; set; }
}
