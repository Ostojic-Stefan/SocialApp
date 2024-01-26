using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Extensions;
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

    public override async Task<Result<UserDetailsResponse>> Handle(GetUserInformationByUsernameQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<UserDetailsResponse>();
        try
        {
            var userRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var user = await userRepo
                    .Query()
                    .Include(u => u.Posts)
                    .Include(u => u.ProfileImage)
                    .Include(u => u.Friends)
                    .Include(u => u.SentFriendRequests)
                    .Include(u => u.ReceivedFriendRequests)
                    .Where(u => u.Username == request.Username)
                    .AsSplitQuery()
                    .ToDetailsResponse(request.CurrentUserId)
                    .SingleOrDefaultAsync(cancellationToken);
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
