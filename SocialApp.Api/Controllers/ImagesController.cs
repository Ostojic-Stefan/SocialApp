using EfCoreHelpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Requests.Posts;
using SocialApp.Api.Requests.UserProfiles;
using SocialApp.Application.Files.Commands;
using SocialApp.Application.Services;

namespace SocialApp.Api.Controllers;

public class ImagesController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment env;
    private readonly IUnitOfWork unitOfWork;

    public ImagesController(IMediator mediator, IWebHostEnvironment env, IUnitOfWork unitOfWork)
    {
        this.env = env;
        this.unitOfWork = unitOfWork;
        _mediator = mediator;
    }


    [HttpPost]
    [Route("users/images")]
    public async Task<IActionResult> UploadProfileImage([FromForm] UploadUserProfileImageRequest uploadUserProfileImage,
        CancellationToken cancellationToken)
    {
        var command = new UploadUserProfileImageCommand
        {
            ImageStream = uploadUserProfileImage.Img.OpenReadStream(),
            ImageName = $"{uploadUserProfileImage.Img.FileName}"
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPost]
    [Route("posts/images")]
    public async Task<IActionResult> UploadPostImage([FromForm] UploadPostImageRequest uploadPostImage,
        CancellationToken cancellationToken)
    {
        var command = new UploadPostImageCommand
        {
            ImageStream = uploadPostImage.Img.OpenReadStream(),
            ImageName = $"{uploadPostImage.Img.FileName}"
        };
        var response = await _mediator.Send(command, cancellationToken);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }
}