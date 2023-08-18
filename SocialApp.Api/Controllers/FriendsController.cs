using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Filters;
using SocialApp.Api.Requests.Friends;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Application.UserProfiles.Queries;

namespace SocialApp.Api.Controllers;

[Authorize]
public class FriendsController : BaseApiController
{
    private readonly IMediator _mediator;
    public FriendsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("users/{userId}/friendRequests")]
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
        return Ok(response.Data);
    }

    [HttpPut]
    [Route("users/{userId}/friendRequests")]
    [ValidateGuids("userId")]
    public async Task<IActionResult> UpdateFriendRequest(string userId,
        UpdateFriendRequest updateFriendRequest, CancellationToken cancellationToken)
    {
        var command = new UpdateFriendRequestCommand
        {
            CurrentUser = HttpContext.GetUserProfileId(),
            OtherUser = Guid.Parse(userId),
            Status = updateFriendRequest.Status
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [Route("users/{userId}/friends")]
    [HttpGet]
    [ValidateGuids("userId")]
    public async Task<IActionResult> GetAllFriends(string userId, CancellationToken cancellationToken)
    {
        var query = new GetAllFriendsQuery
        {
            UserId = Guid.Parse(userId)
        };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}