using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Services;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialApp.Application.Identity.CommandHandlers;

internal class RegisterCommandHandler
    : DataContextRequestHandler<RegisterCommand, Result<IdentityResponse>>
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

    public override async Task<Result<IdentityResponse>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IdentityResponse>();
        try
        {
            var identity = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Email,
            };

            var results = await Task.WhenAll(
                _userManager.FindByEmailAsync(request.Email),
                _userManager.FindByNameAsync(request.Username)
            );

            if (results.Any(user => user is not null))
            {
                var errors = new List<string>(2);
                if (results[0] is not null)
                    errors.Add($"User with the email of {request.Email} already exists");
                if (results[1] is not null)
                    errors.Add($"User with the username of {request.Username} already exists");
                result.AddError(AppErrorCode.DuplicateEntry, errors.ToArray());
                return result;
            }

            // create identity user
            var identityResult = await _userManager.CreateAsync(identity, request.Password);

            if (!identityResult.Succeeded)
            {
                result.AddError(AppErrorCode.BadCredentials,
                    identityResult.Errors.Select(err => err.Description).ToArray());
                return result;
            }

            // create user profile
            var userProfile = UserProfile.CreateUserProfle(identity.Id, request.Username, request.Biography);

            var userProfileRepo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            userProfileRepo.Add(userProfile);
            await _unitOfWork.SaveAsync(cancellationToken);

            // generate jwt
            string token = _tokenService.GetToken(new Claim[]
            {
                new(JwtRegisteredClaimNames.Sub, identity.Email),
                new(JwtRegisteredClaimNames.Email, identity.Email),
                new("IdentityId", identity.Id),
                new("UserProfileId", userProfile.Id.ToString()),
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
