using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Extensions;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Application.UserProfiles.Extensions;
using SocialApp.Application.UserProfiles.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetPostByIdQueryHandler
    : DataContextRequestHandler<GetPostByIdQuery, Result<PostResponse>>
{
    private readonly IMapper _mapper;

    public GetPostByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<PostResponse>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new Result<PostResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var post = await postRepo
                .QueryById(request.PostId)
                .TagWith($"[{nameof(GetPostByIdQueryHandler)}] - Get Single Post")
                .Select((post) => post.MapToPostReponse(request.CurrentUserId))
                .SingleOrDefaultAsync(cancellationToken);
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
