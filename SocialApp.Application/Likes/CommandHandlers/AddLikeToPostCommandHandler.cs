using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Likes.Commands;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Likes.CommandHandlers;

internal class AddLikeToPostCommandHandler
    : DataContextRequestHandler<AddLikeToPostCommand, Result<PostLikeResponse>>
{
    private readonly IMapper _mapper;

    public AddLikeToPostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostLikeResponse>> Handle(AddLikeToPostCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostLikeResponse>();
        try
        {
            var repo = _unitOfWork.CreateReadWriteRepository<PostLike>();
            var postLikeExists = await repo.Query()
                .Where(
                    pl => pl.PostId == request.PostId &&
                    pl.UserProfileId == request.UserProfileId)
                .SingleOrDefaultAsync(cancellationToken);
            if (postLikeExists is not null)
            {
                result.AddError(AppErrorCode.DuplicateEntry, $"You Have already liked this post");
                return result;
            }
            var postLike = PostLike.Create(request.PostId,
                request.UserProfileId,
                request.LikeReaction);
            repo.Add(postLike);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = _mapper.Map<PostLikeResponse>(postLike);
        }
        catch (ModelInvalidException ex)
        {
            result.AddError(AppErrorCode.ValidationError, ex.ValidationErrors);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
