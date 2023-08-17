using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Filters;
using SocialApp.Api.Requests.Comments;
using SocialApp.Application.Comments.Commands;
using SocialApp.Application.Comments.Query;

namespace SocialApp.Api.Controllers;

public class CommentsController : BaseApiController
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("posts/{postId}/comments")]
    [Authorize]
    [ValidateModel]
    public async Task<IActionResult> AddCommentToPost(CreateComment createComment,
        CancellationToken cancellationToken)
    {
        var userProfileId = HttpContext.GetUserProfileId();
        var command = new CreateCommentCommand
        {
            UserProfileId = userProfileId,
            Contents = createComment.Contents,
            PostId = createComment.PostId
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return CreatedAtAction(nameof(GetCommentById), new { commentId = response.Data.Id }, response.Data);
    }

    [HttpGet]
    [Route("posts/{postId}/comments")]
    [ValidateGuids("postId")]
    [Authorize]
    public async Task<IActionResult> GetAllCommentsFromPost(Guid postId,
        CancellationToken cancellationToken)
    {
        var query = new GetCommentsFromPostQuery 
        { 
            PostId = postId,
            CurrentUserId = HttpContext.GetUserProfileId()
        };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpGet]
    [Route("comments/{commentId}")]
    [ValidateGuids("commentId")]
    public async Task<IActionResult> GetCommentById(Guid commentId,
        CancellationToken cancellationToken)
    {
        var query = new GetCommentByIdQuery { CommentId = commentId };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}
