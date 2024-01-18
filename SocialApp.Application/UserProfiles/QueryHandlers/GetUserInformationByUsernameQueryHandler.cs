using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Queries;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.QueryHandlers;

internal class GetUserInformationByUsernameQueryHandler
    : DataContextRequestHandler<GetUserInformationByUsernameQuery,
        Result<UserDetailsResponse>>
{
    private readonly IMapper _mapper;

    public GetUserInformationByUsernameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<UserDetailsResponse>> Handle(
        GetUserInformationByUsernameQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<UserDetailsResponse>();
        try
        {
            var userRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            //var user = await userRepo
            //    .Query()
            //    .Where(u => u.Username == request.Username)
            //    .ProjectTo<UserInformationResponse>(_mapper.ConfigurationProvider)
            //    .SingleOrDefaultAsync(cancellationToken);
            var user = await userRepo
                    .Query()
                    .Where(u => u.Username == request.Username)
                    .Select(user => new UserDetailsResponse
                    {
                        UserInfo = new UserInfo
                        {
                            Username = user.Username,
                            AvatarUrl = user.AvatarUrl,
                            Biography = user.Biography,
                            UserProfileId = user.Id
                        },
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                        IsFriend = user.Friends.Any(f => f.Id == request.CurrentUserId),
                    }).SingleOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                result.AddError(AppErrorCode.NotFound,
                    $"User with username {request.Username} does not exist");
                return result;
            }
            result.Data = user;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
