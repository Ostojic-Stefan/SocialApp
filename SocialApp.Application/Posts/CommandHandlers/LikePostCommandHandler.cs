using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Interfaces;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class LikePostCommandHandler : DataContextRequestHandler<LikePostCommand, Result<PostLikeAddResponse>>
{
    private readonly IMapper _mapper;
    private readonly INotificationMessenger _notificationMessenger;

    public LikePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationMessenger notificationMessenger) 
        : base(unitOfWork)
    {
        _mapper = mapper;
        _notificationMessenger = notificationMessenger;
    }

    public override async Task<Result<PostLikeAddResponse>> Handle(LikePostCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostLikeAddResponse>();
        try
        {
            var repo = _unitOfWork.CreateReadWriteRepository<Post>();
            var post = await repo.Query()
                .Include(p => p.Likes)
                .ThenInclude(l => l.UserProfile)
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(p => p.Id == request.PostId, cancellationToken);
            if (post is null)
            {
                result.AddError(AppErrorCode.NotFound, $"Post with id of {request.PostId} does not exist");
                return result;
            }
            if (post.Likes.Any(l => l.UserProfile.Id == request.UserProfileId))
            {
                result.AddError(AppErrorCode.DuplicateEntry, $"You Have already liked this post");
                return result;
            }
            var postLike = PostLike.Create(request.PostId, request.UserProfileId, request.LikeReaction);
            post.AddLike(postLike);
            await _unitOfWork.SaveAsync(cancellationToken);

            // TODO: temporary workaround
            var likeRepo = _unitOfWork.CreateReadOnlyRepository<PostLike>();
            var tmpLike = await likeRepo.QueryById(postLike.Id).Include(like => like.UserProfile).SingleAsync(cancellationToken);

            result.Data = new PostLikeAddResponse
            {
                LikeId = postLike.Id,
                PostId = post.Id
            };
            await _notificationMessenger.AddAsync(new LikeNotificationMessage
            {
                 Like = tmpLike,
                 Post = post,
                 SenderUserId = request.UserProfileId,
            }, cancellationToken);
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
