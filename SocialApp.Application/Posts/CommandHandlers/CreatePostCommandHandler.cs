using EfCoreHelpers;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Posts.CommandHandlers;

internal class CreatePostCommandHandler
    : DataContextRequestHandler<CreatePostCommand, Result<Post>>
{
    public CreatePostCommandHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<Post>> Handle(CreatePostCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<Post>();
        try
        {
            var post = Post.CreatePost(request.ImageUrl, request.Contents, request.UserProfileId);
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
            postRepo.Add(post);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = post;
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
