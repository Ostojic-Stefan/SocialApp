using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Identity.Commands;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.CommandHandlers;


internal class AddProfileImageCommandHandler
    : DataContextRequestHandler<AddProfileImageCommand, Result<bool>>
{
    public AddProfileImageCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<bool>> Handle(AddProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var userProfileRepo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            var userProfile = await userProfileRepo.Query()
                .TagWith($"[{nameof(AddProfileImageCommandHandler)}] update image")
                .SingleAsync(u => u.Id == request.UserProfileId, cancellationToken);
            userProfile.UpdateUserProfile(request.ImageUrl);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = true;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
