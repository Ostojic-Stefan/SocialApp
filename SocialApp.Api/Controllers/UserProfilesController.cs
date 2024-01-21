using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Requests.UserProfiles;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Application.UserProfiles.Queries;

namespace SocialApp.Api.Controllers;

public class UserProfilesController : BaseApiController
{
    private readonly IMediator _mediator;

    public UserProfilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("users/images/add")]
    [Authorize]
    public async Task<IActionResult> AddImage(AddUserImage avatarUrl)
    {
        var command = new AddUserImageCommand
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
    [Authorize]

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

    [HttpPost]
    [Route("/users/images/set")]
    [Authorize]

    public async Task<IActionResult> SetProfileImage(SetUserProfileImage request)
    {
        var command = new SetUserProfileImageCommand
        {
            CurrentUserId = HttpContext.GetUserProfileId(),
            ImageId = request.ImageId
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpGet]
    [Route("/users/{userId}/images")]
    [Authorize]
    public async Task<IActionResult> GetImagesForUser(Guid userId)
    {
        var command = new GetAllImagesQuery
        {
            CurrentUserId = HttpContext.GetUserProfileId(),
            UserProfileId = userId
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}
