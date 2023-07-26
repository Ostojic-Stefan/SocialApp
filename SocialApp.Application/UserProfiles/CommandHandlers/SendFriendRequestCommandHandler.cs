using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.CommandHandlers;

internal class SendFriendRequestCommandHandler
    : DataContextRequestHandler<SendFriendRequestCommand, Result<bool>>
{
    public SendFriendRequestCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<bool>> Handle(SendFriendRequestCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var repo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            var receiverUser = await repo
                .Query()
                .Include(u => u.ReceivedFriendRequests.Where(x => x.Status == FriendRequestStatus.Pending))
                .Include(u => u.Friends)
                .SingleAsync(u => u.Id == request.RecieverUser, cancellationToken);
            if (receiverUser is null)
            {
                result.AddError(AppErrorCode.NotFound,
                    $"User with the id of {request.RecieverUser} does not exist");
                return result;
            }
            if (receiverUser.ReceivedFriendRequests.Any(f => f.SenderUserId == request.CurrentUser))
            {
                result.AddError(AppErrorCode.DuplicateEntry,
                    $"You have already sent a request to the user: {request.RecieverUser}");
                return result;
            }
            if (receiverUser.Friends.Any(f => f.Id == request.CurrentUser))
            {
                result.AddError(AppErrorCode.NotFound,
                    $"You are already friends with {request.RecieverUser}");
                return result;
            }
            receiverUser.SendFriendRequest(request.CurrentUser);
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
