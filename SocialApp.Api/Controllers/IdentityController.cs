using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Extensions;
using SocialApp.Api.Requests.Identity;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Identity.Queries;

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
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);
        var result = await _mediator.Send(command);
        if (result.HasError)
            return HandleError(result.Errors);
        return Ok(result.Data);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var command = _mapper.Map<LoginCommand>(loginRequest);
        var result = await _mediator.Send(command);
        if (result.HasError)
            return HandleError(result.Errors);
        return Ok(result.Data);
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
}
