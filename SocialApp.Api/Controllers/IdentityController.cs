using AutoMapper;
using DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Api.Contracts.Identity;
using SocialApp.Application.Identity.Commands;
using SocialApp.Domain;

namespace SocialApp.Api.Controllers;

public class IdentityController : BaseApiController
{
    private readonly DataContext _ctx;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public IdentityController(DataContext ctx, IMediator mediator, IMapper mapper)
    {
        _ctx = ctx;
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("/register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);
        var result = await _mediator.Send(command);
        if (result.HasError)
            return HandleError(result.Errors);
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser()
    {
        var userProfile = UserProfile.CreateUserProfle("test", "test", "test", "tets");
        _ctx.UserProfiles.Add(userProfile);
        await _ctx.SaveChangesAsync();
        return Ok();
    }
}
