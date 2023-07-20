using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SocialApp.Api.Extensions;
using SocialApp.Api.Requests.Identity;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Identity.Queries;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Settings;

namespace SocialApp.Api.Controllers;

public class IdentityController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _environment;
    private readonly JwtSettings _jwtSettings;

    public IdentityController(IMediator mediator, IMapper mapper,
        IOptions<JwtSettings> jwtOptions,
        IWebHostEnvironment env)
    {
        _mediator = mediator;
        _mapper = mapper;
        _environment = env;
        _jwtSettings = jwtOptions.Value;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);
        var result = await _mediator.Send(command);
        if (result.HasError)
            return HandleError(result.Errors);
        var token = result.Data.AccessToken;
        Response.Cookies.Append(_jwtSettings.CookieName, token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        });
        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var command = _mapper.Map<LoginCommand>(loginRequest);
        var result = await _mediator.Send(command);
        if (result.HasError)
            return HandleError(result.Errors);
        var token = result.Data.AccessToken;
        Response.Cookies.Append(_jwtSettings.CookieName, token, new CookieOptions
        { 
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Secure = true
        });
        return Ok();
    }

    [HttpGet]
    [Route("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userProfileId = HttpContext.GetUserProfileId();
        var result = await _mediator.Send(
            new GetUserInformationQuery { UserProfileId = userProfileId });
        if (result.HasError)
            return HandleError(result.Errors);
        return Ok(result.Data);
    }

    [HttpPost]
    [Route("uploadImage")]
    [Authorize]
    public async Task<IActionResult> UploadProfileImage(IFormFile img)
    {
        var command = new UploadProfileImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageStream = img.OpenReadStream(),
            DirPath = $"{_environment.WebRootPath}\\User",
            ImageName = $"{img.FileName}"
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

    [HttpPost]
    [Route("setImage")]
    [Authorize]
    public async Task<IActionResult> AddProfileImage([FromBody] string imgUrl)
    {
        var command = new AddProfileImageCommand
        {
            UserProfileId = HttpContext.GetUserProfileId(),
            ImageUrl = imgUrl
        };
        var response = await _mediator.Send(command);
        if (response.HasError)
            return HandleError(response.Errors);
        return Ok(response.Data);
    }

}
