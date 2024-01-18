using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Requests.Identity;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Application.UserProfiles.Queries;

namespace SocialApp.Api.Controllers;

// TODO: Add Update Endpoint
public class UserProfilesController : BaseApiController
{
    private readonly IMediator _mediator;

    public UserProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("users/setImage")]
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

    [HttpGet]
    [Route("users/{username}")]
    public async Task<IActionResult> GetUserInfomation(string username)
    {
        var query = new GetUserInformationByUsernameQuery
        {
            CurrentUserId = HttpContext.GetUserProfileId(),
            Username = username
        };
        var response = await _mediator.Send(query);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}
