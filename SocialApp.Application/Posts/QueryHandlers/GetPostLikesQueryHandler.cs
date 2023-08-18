using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Models;
using SocialApp.Application.Posts.Queries;
using SocialApp.Application.Posts.Responses;
using SocialApp.Domain;

namespace SocialApp.Application.Posts.QueryHandlers;

internal class GetPostLikesQueryHandler
    : DataContextRequestHandler<GetPostLikesQuery, Result<GetLikesForAPostResponse>>
{
    private readonly IMapper _mapper;

    public GetPostLikesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
            _mapper = mapper;
    }

    public override async Task<Result<GetLikesForAPostResponse>> Handle(GetPostLikesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new Result<GetLikesForAPostResponse>();
        try
        {
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();
            var post = await postRepo
                .QueryById(request.PostId)
                .Include(p => p.Likes)
                .ThenInclude(p => p.UserProfile)
                .FirstAsync(cancellationToken);

            // TODO: Move elsewhere
            if (post.UserProfileId == request.CurrentUser)
            {
                foreach (var like in post.Likes)
                    like.SetLikeAsSeen();
            }
            
            result.Data = _mapper.Map<GetLikesForAPostResponse>(post);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}