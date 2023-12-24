using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.CommandHandlers;


internal class AddProfileImageCommandHandler
    : DataContextRequestHandler<AddProfileImageCommand, Result<string>>
{
    public AddProfileImageCommandHandler(IUnitOfWork unitOfWork)
        : base(unitOfWork)
    {
    }

    public override async Task<Result<string>> Handle(AddProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<string>();
        try
        {
            var userProfileRepo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            var userProfile = await userProfileRepo.Query()
                .TagWith($"[{nameof(AddProfileImageCommandHandler)}] update image")
                .SingleAsync(u => u.Id == request.UserProfileId, cancellationToken);
            userProfile.UpdateUserProfile(request.ImageUrl);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = userProfile.AvatarUrl;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
