using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Models;
using SocialApp.Application.Services;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialApp.Application.Identity.CommandHandlers;

internal class RegisterCommandHandler
    : DataContextRequestHandler<RegisterCommand, Result<string>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        ITokenService tokenService
        ) 
        : base(unitOfWork)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public override async Task<Result<string>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<string>();
        try
        {
            var identity = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
            };

            if (await _userManager.FindByEmailAsync(request.Email) is not null)
            {
                result.AddError(AppErrorCode.UserAlreadyExists,
                    $"User with the email of {request.Email} already exists");
                return result;
            }

            // create identity user
            var identityResult = await _userManager.CreateAsync(identity, request.Password);

            if (!identityResult.Succeeded)
            {
                result.AddError(AppErrorCode.ServerError,
                    identityResult.Errors.Select(err => err.Description).ToArray());
                return result;
            }

            // create user profile
            var userProfile = UserProfile.CreateUserProfle(
                identity.Id, request.Username,
                request.Biography, request.AvatarUrl);

            var userProfileRepo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            userProfileRepo.Add(userProfile);
            await _unitOfWork.SaveAsync(cancellationToken);

            // generate jwt
            string token = _tokenService.GetToken(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                new Claim("IdentityId", identity.Id),
                new Claim("UserProfileId", userProfile.Id.ToString()),
            });

            result.Data = token;
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
