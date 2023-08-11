using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Filters;
using SocialApp.Api.Requests.Likes;
using SocialApp.Application.Likes.Commands;
using SocialApp.Application.Likes.Queries;
using SocialApp.Application.Posts.Commands;

namespace SocialApp.Api.Controllers;

public class LikesController : BaseApiController
{
    private readonly IMediator _mediator;

    public LikesController(IMediator mediator)
	{
        _mediator = mediator;
    }

    [HttpPost]
    [Route("posts/{postId}")]
    [ValidateGuids("postId")]
    public async Task<IActionResult> AddLikeToPost(string postId,
        AddLikeRequest addLikeRequest, CancellationToken cancellationToken)
    {
        var command = new LikePostCommand
        {
            LikeReaction = addLikeRequest.LikeReaction,
            PostId = Guid.Parse(postId),
            UserProfileId = HttpContext.GetUserProfileId(),
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpGet]
    [Route("posts/{postId}")]
    [ValidateGuids("postId")]
    [Authorize]
    public async Task<IActionResult> GetAllLikesForAPost(string postId, CancellationToken cancellationToken)
    {
        var query = new GetLikesForAPostQuery
        {
            PostId = Guid.Parse(postId),
            CurrentUser = HttpContext.GetUserProfileId()
        };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpGet]
    [Route("users/{username}")]
    [ValidateGuids("postId")]
    public async Task<IActionResult> GetAllLikesByUser(string username, CancellationToken cancellationToken)
    {
        var query = new GetLikesByUserQuery
        {
            Username = username
        };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpDelete]
    [Route("{likeId}")]
    [ValidateGuids("likeId")]
    public async Task<IActionResult> DeleteLike(string likeId, CancellationToken cancellationToken)
    {
        var command = new DeleteLikeCommand
        {
            LikeId = Guid.Parse(likeId),
            UserProfileId = HttpContext.GetUserProfileId()
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}
