using AutoMapper;
using EfCoreHelpers;
using SocialApp.Application.Comments.Commands;
using SocialApp.Application.Comments.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Comments.CommandHandlers;

internal class CreateCommentCommandHandler
    : DataContextRequestHandler<CreateCommentCommand, Result<CommentResponse>>
{
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
       _mapper = mapper;
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
            var commentRepo = _unitOfWork
                .CreateReadWriteRepository<Comment>();
            commentRepo.Add(comment);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = _mapper.Map<CommentResponse>(comment);
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
