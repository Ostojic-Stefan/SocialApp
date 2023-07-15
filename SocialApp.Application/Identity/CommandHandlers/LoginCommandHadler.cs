using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Services;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialApp.Application.Identity.CommandHandlers;
internal class LoginCommandHadler 
    : DataContextRequestHandler<LoginCommand, Result<IdentityResponse>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHadler(IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        ITokenService tokenService
    ) 
        : base(unitOfWork)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public override async Task<Result<IdentityResponse>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IdentityResponse>();
        try
        {
            var identity = await _userManager.FindByEmailAsync(request.Email);
            if (identity is null)
            {
                result.AddError(AppErrorCode.BadCredentials, "Wrong Email Address");
                return result;
            }
            if (!await _userManager.CheckPasswordAsync(identity, request.Password))
            {
                result.AddError(AppErrorCode.BadCredentials, "Wrong Password");
                return result;
            }
            var userProfileRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var userProfile = await userProfileRepo.Query()
                .TagWith("Get user profile - Login")
                .Select(u => new { u.Id, u.IdentityId })
                .SingleOrDefaultAsync(u => u.IdentityId == identity.Id, cancellationToken);
            var token = _tokenService.GetToken(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                new Claim("IdentityId", identity.Id),
                new Claim("UserProfileId", userProfile.Id.ToString()),
            });
            result.Data = new IdentityResponse { AccessToken = token };
        }
        catch (ModelInvalidException ex)
        {
            result.AddError(AppErrorCode.ValidationError, ex.ValidationErrors);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
