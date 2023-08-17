using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Requests.Posts;
using SocialApp.Api.Requests.UserProfiles;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.UserProfiles.Commands;

namespace SocialApp.Api.Controllers;

public class ImagesController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;

    public ImagesController(IMediator mediator, IWebHostEnvironment environment)
    {
        _mediator = mediator;
        _environment = environment;
    }

    [HttpPost]
    [Route("users/images")]
    public async Task<IActionResult> UploadProfileImage([FromForm] UploadUserProfileImageRequest uploadUserProfileImage,
        CancellationToken cancellationToken)
    {
        var command = new UploadProfileImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageStream = uploadUserProfileImage.Img.OpenReadStream(),
            DirPath = $"{_environment.WebRootPath}/User",
            ImageName = $"{uploadUserProfileImage.Img.FileName}"
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPost]
    [Route("posts/images")]
    public async Task<IActionResult> UploadPostImage([FromForm] UploadPostImageRequest uploadPostImage)
    {
        var command = new UploadImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageStream = uploadPostImage.Img.OpenReadStream(),
            DirPath = $"{_environment.WebRootPath}/Posts",
            ImageName = $"{uploadPostImage.Img.FileName}"
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}