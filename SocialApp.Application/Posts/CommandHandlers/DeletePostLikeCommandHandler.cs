using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class DeletePostLikeCommandHandler
    : DataContextRequestHandler<DeletePostLikeCommand, Result<PostLikeDeleteResponse>>
{
    private readonly IMapper _mapper;

    public DeletePostLikeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostLikeDeleteResponse>> Handle(DeletePostLikeCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostLikeDeleteResponse>();
        try
        {
            var postLikeRepo = _unitOfWork.CreateReadWriteRepository<PostLike>();
            var like = await postLikeRepo.GetByIdAsync(request.LikeId, cancellationToken);

            if (like is null)
            {
                result.AddError(AppErrorCode.NotFound, "Provided like does not exist");
                return result;
            }
            if (like.UserProfileId != request.UserProfileId)
            {
                result.AddError(AppErrorCode.UnAuthorized, "You can only delete your own likes");
                return result;
            }
            var postLike = await postLikeRepo.RemoveAsync(request.LikeId, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = _mapper.Map<PostLikeDeleteResponse>(like);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
            throw;
        }
        
        return result;
    }
}
