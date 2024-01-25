using EfCoreHelpers;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileCommandHandler
    : DataContextRequestHandler<UpdateUserProfileCommand, Result<bool>>
{
    public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<bool>> Handle(UpdateUserProfileCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var userRepo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            var user = await userRepo.GetByIdAsync(request.CurrentUserId, cancellationToken);
            if (user is null)
            {
                result.AddError(AppErrorCode.NotFound, "User Not Found");
                return result;
            }
            user.UpdateUserProfile(request.Username, request.Biography);
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
