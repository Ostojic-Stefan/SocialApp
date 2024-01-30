using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Filters;
using SocialApp.Api.Requests.Identity;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Identity.Queries;
using System.Security.Claims;

namespace SocialApp.Api.Controllers;

public class IdentityController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentityController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("identity/register")]
    [ValidateModel]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);
        var result = await _mediator.Send(command);
        if (result.HasError)
            return HandleError(result.Errors);
        return Ok(result.Data);
    }

    [HttpPost]
    [Route("identity/login")]
    [ValidateModel]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var command = _mapper.Map<LoginCommand>(loginRequest);
        var result = await _mediator.Send(command);
        if (result.HasError)
            return HandleError(result.Errors);

        var authOptions = new AuthenticationProperties
        {
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            "Cookie",
            new ClaimsPrincipal(result.Data.ClaimsIdentity), authOptions);

        return Ok();
    }


    [HttpPost]
    [Route("identity/logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookie");
        return Ok();
    }

    [HttpGet]
    [Route("identity/me")]
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
}
