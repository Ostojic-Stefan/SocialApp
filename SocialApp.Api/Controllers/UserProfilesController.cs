using EfCoreHelpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.Api.Extensions;
using SocialApp.Api.Filters;
using SocialApp.Api.Requests.Identity;
using SocialApp.Api.Requests.UserProfiles;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Application.UserProfiles.Queries;
using SocialApp.Domain;

namespace SocialApp.Api.Controllers;

public class UserProfilesController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _environment;
    private readonly IUnitOfWork unitOfWork;

    public UserProfilesController(IMediator mediator, IWebHostEnvironment env, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _environment = env;
        this.unitOfWork = unitOfWork;
    }

    [HttpPost]
    [Route("uploadImage")]
    [Authorize]
    [ValidateModel]
    public async Task<IActionResult> UploadProfileImage([FromForm] UploadUserProfileImageRequest uploadUserProfileImage)
    {
        var command = new UploadProfileImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageStream = uploadUserProfileImage.Img.OpenReadStream(),
            DirPath = $"{_environment.WebRootPath}\\User",
            ImageName = $"{uploadUserProfileImage.Img.FileName}"
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

    [HttpGet]
    [Route("getInformation/{username}")]
    public async Task<IActionResult> GetUserInfomation(string username)
    {
        var query = new GetUserInformationByUsernameQuery
        {
            Username = username
        };
        var response = await _mediator.Send(query);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPost]
    [Route("sendFriendRequest/{userId}")]
    [Authorize]
    [ValidateGuids("userId")]
    public async Task<IActionResult> SendFriendRequest(string userId, CancellationToken cancellationToken)
    {
        var command = new SendFriendRequestCommand
        {
            CurrentUser = HttpContext.GetUserProfileId(),
            RecieverUser = Guid.Parse(userId)
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok();
    }

    [HttpPost]
    [Route("acceptFriendRequest/{userId}")]
    [Authorize]
    [ValidateGuids("userId")]
    public async Task<IActionResult> AcceptFriendRequest(string userId, CancellationToken cancellationToken)
    {
        var command = new AcceptFriendRequestCommand
        {
            CurrentUser = HttpContext.GetUserProfileId(),
            OtherUser = Guid.Parse(userId)
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok();
    }
}
