using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Commands;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;
using SocialApp.Application.Services.BackgroundServices.Notification;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Comments.CommandHandlers;

internal class CreateCommentCommandHandler
    : DataContextRequestHandler<CreateCommentCommand, Result<CommentResponse>>
{
    private readonly IMapper _mapper;
    private readonly INotificationQueue _notificationQueue;

    public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationQueue notificationQueue) 
        : base(unitOfWork)
    {
       _mapper = mapper;
        _notificationQueue = notificationQueue;
    }

    public override async Task<Result<CommentResponse>> Handle(CreateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<CommentResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var post = await postRepo.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
            {
                result.AddError(AppErrorCode.NotFound, $"post with id of {request.PostId} does not exist");
                return result;
            }
            var comment = Comment.CreateComment(request.Contents, request.UserProfileId, request.PostId);
            var commentRepo = _unitOfWork.CreateReadWriteRepository<Comment>();
            commentRepo.Add(comment);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = _mapper.Map<CommentResponse>(await commentRepo
                .QueryById(comment.Id)
                .Include(c => c.UserProfile)
                .FirstAsync(cancellationToken)
            );

            _notificationQueue.AddNotification(new QueueData(request.UserProfileId, post, comment, null));
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
