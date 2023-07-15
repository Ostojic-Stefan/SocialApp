using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Identity.Queries;
using SocialApp.Application.Identity.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Identity.QueryHandlers;

internal class GetUserInformationQueryHandler
    : DataContextRequestHandler<GetUserInformationQuery, Result<GetUserInformationResponse>>
{
    private readonly IMapper _mapper;

    public GetUserInformationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<GetUserInformationResponse>> Handle(GetUserInformationQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<GetUserInformationResponse>();
        try
        {
            var userProfileRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();

            var userProfile = await userProfileRepo
                .QueryById(request.UserProfileId)
                .TagWith($"[{nameof(GetUserInformationQueryHandler)}] - Get user profile information")
                .ProjectTo<GetUserInformationResponse>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (userProfile is null)
            {
                result.AddError(AppErrorCode.NotFound,
                    $"User with the id of {request.UserProfileId} does not exist");
                return result;
            }
            result.Data = userProfile;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
