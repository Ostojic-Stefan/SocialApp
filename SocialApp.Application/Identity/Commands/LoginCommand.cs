using MediatR;
using SocialApp.Application.Models;

namespace SocialApp.Application.Identity.Commands;

public class LoginCommand : IRequest<Result<string>>
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
