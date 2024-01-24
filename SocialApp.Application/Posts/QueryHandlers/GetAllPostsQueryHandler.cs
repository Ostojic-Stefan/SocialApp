using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Extensions;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetAllPostsQueryHandler
    : DataContextRequestHandler<GetAllPostsQuery, Result<IReadOnlyList<PostResponse>>>
{
    private readonly IMapper _mapper;

    public GetAllPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<PostResponse>>> Handle(GetAllPostsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<PostResponse>>();
        try
        {
            var repo = _unitOfWork.CreateReadOnlyRepository<Post>();
            var likeRepo = _unitOfWork.CreateReadOnlyRepository<PostLike>();
            var posts = await repo
             .Query()
             .Where(p => p.DoneProcessing)
             .Include(p => p.Images)
             .Include(p => p.Likes)
             .Include(p => p.Comments)
             .Include(p => p.UserProfile.ProfileImage)
             .OrderByDescending(p => p.CreatedAt)
             .Select(p => p.MapToPostReponse(request.CurrentUserId))
             .ToListAsync(cancellationToken);
            result.Data = posts;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
