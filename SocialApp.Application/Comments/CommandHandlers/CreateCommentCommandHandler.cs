using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Comments.Commands;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Interfaces;
using SocialApp.Application.Models;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Comments.CommandHandlers;

internal class CreateCommentCommandHandler
    : DataContextRequestHandler<CreateCommentCommand, Result<CommentResponse>>
{
    private readonly IMapper _mapper;
    private readonly INotificationMessenger _notificationMessenger;

    public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, INotificationMessenger notificationMessenger) 
        : base(unitOfWork)
    {
        _mapper = mapper;
        _notificationMessenger = notificationMessenger;
    }

    public override async Task<Result<CommentResponse>> Handle(CreateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<CommentResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var post = await postRepo.QueryById(request.PostId).Include(p => p.UserProfile).SingleAsync(cancellationToken);
            if (post is null)
            {
                result.AddError(AppErrorCode.NotFound, $"post with id of {request.PostId} does not exist");
                return result;
            }
            var comment = Comment.CreateComment(request.Contents, request.UserProfileId, request.PostId);
            var commentRepo = _unitOfWork.CreateReadWriteRepository<Comment>();
            commentRepo.Add(comment);
            await _unitOfWork.SaveAsync(cancellationToken);
            // TODO: fix when loading profile image for user
            result.Data = _mapper.Map<CommentResponse>(await commentRepo
                .QueryById(comment.Id)
                .Include(c => c.UserProfile)
                .FirstAsync(cancellationToken)
            );

            await _notificationMessenger.AddAsync(new CommentNotificationMessage
            {
                Comment = comment,
                Post = post,
                SenderUserId = request.UserProfileId
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
