using EfCoreHelpers;
using SocialApp.Application.Identity.Queries;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Identity.QueryHandlers;

internal class GetUserInformationQueryHandler
    : DataContextRequestHandler<GetUserInformationQuery, Result<UserProfile>>
{
    public GetUserInformationQueryHandler(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public override async Task<Result<UserProfile>> Handle(GetUserInformationQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<UserProfile>();
        try
        {
            var userProfileRepo = _unitOfWork.CreateReadOnlyRepository<UserProfile>();
            var userProfile = await userProfileRepo
                .GetByIdAsync(request.UserProfileId, cancellationToken);
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
