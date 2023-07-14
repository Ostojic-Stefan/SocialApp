using EfCoreHelpers;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetPostByIdQueryHandler
    : DataContextRequestHandler<GetPostByIdQuery, Result<Post>>
{
    public GetPostByIdQueryHandler(IUnitOfWork unitOfWork) 
        : base(unitOfWork)
    {
    }

    public override async Task<Result<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new Result<Post>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var post = await postRepo.GetByIdAsync(request.PostId, cancellationToken);
            if (post is null)
            {
                result.AddError(
                    AppErrorCode.NotFound,
                    $"Post with id of {request.PostId} does not exist");
                return result;
            }
            result.Data = post;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
