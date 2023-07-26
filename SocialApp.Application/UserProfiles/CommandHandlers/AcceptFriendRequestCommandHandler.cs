using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Domain;

namespace SocialApp.Application.UserProfiles.CommandHandlers;

internal class AcceptFriendRequestCommandHandler
     : DataContextRequestHandler<AcceptFriendRequestCommand, Result<bool>>
{
    public AcceptFriendRequestCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<bool>> Handle(AcceptFriendRequestCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var repo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            var currUser = await repo
                .Query()
                .Include(u => u.ReceivedFriendRequests)
                .ThenInclude(f => f.SenderUser)
                .SingleAsync(u => u.Id == request.CurrentUser, cancellationToken);
            if (!currUser.ReceivedFriendRequests.Any(x => x.SenderUserId == request.OtherUser)) 
            {
                result.AddError(AppErrorCode.NotFound, 
                    $"You have not reveived a friend request from a user with Id: {request.OtherUser}");
                return result;
            }
            currUser.AcceptFriendRequest(request.OtherUser);
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
