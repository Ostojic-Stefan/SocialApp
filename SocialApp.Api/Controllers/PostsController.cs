using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Contracts.Posts.Requests;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Queries;
using SocialApp.Domain;

namespace SocialApp.Api.Controllers;

public class PostsController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PostsController(IMediator mediator, IMapper mapper)
	{
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)
    {
        IReadOnlyList<Post> posts = await _mediator.Send(new GetAllPostsQuery(), cancellationToken);
        return Ok(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(CreatePostRequest createPost, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreatePostCommand>(createPost);
        var response = await _mediator.Send(command, cancellationToken);
        return Ok(response);
    }

}
