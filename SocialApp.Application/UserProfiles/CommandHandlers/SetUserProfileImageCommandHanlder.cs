using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.CommandHandlers;

internal class SetUserProfileImageCommandHanlder
    : DataContextRequestHandler<SetUserProfileImageCommand, Result<bool>>
{
    public SetUserProfileImageCommandHanlder(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<bool>> Handle(SetUserProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var userRepo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            var user = await userRepo
                .QueryById(request.CurrentUserId)
                .Include(u => u.Images)
                .SingleAsync(cancellationToken);
            if (user is null)
            {
                result.AddError(AppErrorCode.NotFound, $"User does not exist");
                return result;
            }
            bool usersImage = user.Images.Any(i => i.Id == request.ImageId);
            if (!usersImage)
            {
                result.AddError(AppErrorCode.NotFound, $"User does not have the provided image");
                return result;
            }
            user.SetProfileImage(request.ImageId);
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
