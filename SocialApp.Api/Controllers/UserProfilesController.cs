using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Requests.Identity;
using SocialApp.Application.UserProfiles.Commands;

namespace SocialApp.Api.Controllers;

public class UserProfilesController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;

    public UserProfilesController(IMediator mediator, IWebHostEnvironment env)
    {
        _mediator = mediator;
        _environment = env;
    }

    [HttpPost]
    [Route("uploadImage")]
    [Authorize]
    public async Task<IActionResult> UploadProfileImage(IFormFile img)
    {
        var command = new UploadProfileImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageStream = img.OpenReadStream(),
            DirPath = $"{_environment.WebRootPath}\\User",
            ImageName = $"{img.FileName}"
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPost]
    [Route("setImage")]
    [Authorize]
    public async Task<IActionResult> SetProfileImage(SetProfileImageRequest avatarUrl)
    {
        var command = new AddProfileImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageUrl = avatarUrl.AvatarUrl
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}
