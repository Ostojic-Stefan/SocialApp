﻿using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;

    public PostsController(IMediator mediator, IMapper mapper, IWebHostEnvironment environment)
	{
        _mediator = mediator;
        _mapper = mapper;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts([FromQuery] PagedRequest pagedRequest,
        CancellationToken cancellationToken)
    {
        var query = new GetAllPostsQuery
        {
            PageNumber = pagedRequest.PageNumber,
            PageSize = pagedRequest.PageSize
        };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpGet]
    [Route("{postId}")]
    [ValidateGuids("postId")]
    public async Task<IActionResult> GetPostById(string postId,
        CancellationToken cancellationToken)
    {
        var query = new GetPostByIdQuery { PostId = Guid.Parse(postId) };
        var response = await _mediator.Send(query, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPost]
    [Authorize]
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
    [Route("{postId}")]
    [Authorize]
    [ValidateGuids("postId")]
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
    [Route("{postId}")]
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

    [HttpPost]
    [Route("upload")]
    [Authorize]
    public async Task<IActionResult> UploadPostImage(IFormFile img)
    {
        var command = new UploadImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageStream = img.OpenReadStream(),
            DirPath = $"{_environment.WebRootPath}\\Posts",
            ImageName = $"{img.FileName}"
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

}
