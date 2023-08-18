using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Filters;
using SocialApp.Api.Requests;
using SocialApp.Api.Requests.Posts;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Queries;

namespace SocialApp.Api.Controllers;

public class PostsController : BaseApiController
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("posts")]
    [Authorize]
    public async Task<IActionResult> GetPosts([FromQuery] PagedRequest pagedRequest,
        CancellationToken cancellationToken)
    {
        var query = new GetAllPostsQuery
        {
            CurrentUserId = HttpContext.GetUserProfileId(),
            PageNumber = pagedRequest.PageNumber,
            PageSize = pagedRequest.PageSize
        };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpGet]
    [Route("posts/{postId}")]
    [Authorize]
    [ValidateGuids("postId")]
    public async Task<IActionResult> GetPostById(string postId,
        CancellationToken cancellationToken)
    {
        var query = new GetPostByIdQuery { PostId = Guid.Parse(postId), CurrentUserId = HttpContext.GetUserProfileId() };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPost]
    [Route("posts")]
    [Authorize]
    [ValidateModel]
    public async Task<IActionResult> CreatePost(CreatePostRequest createPost,
        CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileId();
        var command = new CreatePostCommand
        {
            Contents = createPost.Contents,
            ImageUrl = createPost.ImageUrl,
            UserProfileId = userProfileId,
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPut]
    [Route("posts/{postId}")]
    [Authorize]
    [ValidateGuids("postId")]
    [ValidateModel]
    public async Task<IActionResult> UpdatePost(string postId,
        [FromBody] UpdatePost updatePost, CancellationToken cancellationToken)
    {
        var command = new UpdatePostCommand
        {
            Contents = updatePost.Contents,
            ImageUrl = updatePost.ImageUrl,
            PostId = Guid.Parse(postId),
            UserProfileId = HttpContext.GetUserProfileId()
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpDelete]
    [Route("posts/{postId}")]
    [Authorize]
    [ValidateGuids("postId")]
    public async Task<IActionResult> DeletePost(string postId,
        CancellationToken cancellationToken)
    {
        var command = new DeletePostCommand
        {
            PostId = Guid.Parse(postId),
            UserProfileId = HttpContext.GetUserProfileId()
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }


    [HttpGet]
    [Route("users/{username}/posts")]
    [ValidateModel]
    public async Task<IActionResult> GetPostsForUser(string username)
    {
        var query = new GetPostsForUserQuery
        {
            Username = username
        };
        var response = await _mediator.Send(query);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

}
