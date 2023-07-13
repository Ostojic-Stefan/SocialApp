using DataAccess;
using Microsoft.AspNetCore.Mvc;
using SocialApp.Domain;

namespace SocialApp.Api.Controllers;

public class IdentityController : BaseApiController
{
    private readonly DataContext _ctx;

    public IdentityController(DataContext ctx)
    {
        _ctx = ctx;
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
