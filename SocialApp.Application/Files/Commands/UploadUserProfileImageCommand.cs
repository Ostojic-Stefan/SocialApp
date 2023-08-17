using MediatR;
using SocialApp.Application.Files.Responses;
using SocialApp.Application.Models;

namespace SocialApp.Application.Files.Commands;

public class UploadUserProfileImageCommand : IRequest<Result<UploadUserProfileImageResponse>>
{
    public required string ImageName { get; set; }
    public required Stream ImageStream { get; set; }
}