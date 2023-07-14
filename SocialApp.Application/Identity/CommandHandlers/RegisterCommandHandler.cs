using EfCoreHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Models;
using SocialApp.Application.Settings;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialApp.Application.Identity.CommandHandlers;

internal class RegisterCommandHandler
    : DataContextRequestHandler<RegisterCommand, Result<string>>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<RegisterCommandHandler> _logger;
    private readonly JwtSettings _jwtSettings;

    public RegisterCommandHandler(IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
        ILogger<RegisterCommandHandler> logger,
        IOptions<JwtSettings> jwtOptions
        ) 
        : base(unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _jwtSettings = jwtOptions.Value;
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

            var test = await _userManager.FindByEmailAsync(request.Email);

            if (test is not null)
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
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                new Claim("IdentityId", identity.Id),
                new Claim("UserProfileId", userProfile.Id.ToString()),
            });

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)),
                    SecurityAlgorithms.HmacSha512
                ),
                Audience = _jwtSettings.Audiences[0],
                Issuer = _jwtSettings.Issuer,
                Expires = DateTime.Now.AddHours(2)  
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(descriptor);
            result.Data = tokenHandler.WriteToken(token);
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
