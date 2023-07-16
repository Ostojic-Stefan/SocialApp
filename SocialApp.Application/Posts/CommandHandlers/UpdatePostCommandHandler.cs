using AutoMapper;
using EfCoreHelpers;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Commands;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;
using SocialApp.Domain.Exceptions;

namespace SocialApp.Application.Posts.CommandHandlers;
internal class UpdatePostCommandHandler
    : DataContextRequestHandler<UpdatePostCommand, Result<PostResponse>>
{
    private readonly IMapper _mapper;

    public UpdatePostCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostResponse>> Handle(UpdatePostCommand request,
        CancellationToken cancellationToken)
    {
        var result = new Result<PostResponse>();
        try
        {
            // find post
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
            var post = await postRepo.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
            {
                result.AddError(
                    AppErrorCode.NotFound,
                    $"Post with id of {request.PostId} does not exist");
                return result;
            }

            // check if the user is the creator of the post
            if (post.UserProfileId != request.UserProfileId)
            {
                result.AddError(
                    AppErrorCode.UnAuthorized,
                    "Only the creator of the post can update it");
                return result;
            }

            // update post
            post.Update(request.ImageUrl, request.Contents);
            await _unitOfWork.SaveAsync(cancellationToken);
            result.Data = _mapper.Map<PostResponse>(post);
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
