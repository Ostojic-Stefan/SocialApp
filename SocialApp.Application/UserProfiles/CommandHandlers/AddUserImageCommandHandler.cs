using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Interfaces;
using SocialApp.Application.Models;
using SocialApp.Application.UserProfiles.Commands;
using SocialApp.Domain;
using System.Threading.Channels;

namespace SocialApp.Application.UserProfiles.CommandHandlers;

internal class AddUserImageCommandHandler
    : DataContextRequestHandler<AddUserImageCommand, Result<bool>>
{
    private readonly ChannelWriter<ImageProcessingMessage> _channelWriter;

    public AddUserImageCommandHandler(IUnitOfWork unitOfWork, Channel<ImageProcessingMessage> channel)
        : base(unitOfWork)
    {
        _channelWriter = channel;
    }

    public override async Task<Result<bool>> Handle(AddUserImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<bool>();
        try
        {
            var userProfileRepo = _unitOfWork.CreateReadWriteRepository<UserProfile>();
            var userProfile = await userProfileRepo.Query()
                .TagWith($"[{nameof(AddUserImageCommandHandler)}] update image")
                .SingleAsync(u => u.Id == request.UserProfileId, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            // send to background service for processing
            await _channelWriter.WriteAsync(new ImageProcessingMessage(request.ImageUrl, userProfile.Id, ImageFor.User), cancellationToken);
            result.Data = true;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
