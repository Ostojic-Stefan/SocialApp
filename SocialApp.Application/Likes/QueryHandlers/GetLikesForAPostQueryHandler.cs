using AutoMapper;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Likes.Queries;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Likes.QueryHandlers;

internal class GetLikesForAPostQueryHandler
    : DataContextRequestHandler<GetLikesForAPostQuery,
        Result<IReadOnlyList<GetLikesForAPostResponse>>>
{
    private readonly IMapper _mapper;

    public GetLikesForAPostQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<GetLikesForAPostResponse>>> Handle(
        GetLikesForAPostQuery request, CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<GetLikesForAPostResponse>>();
        try
        {
            var postRepo = _unitOfWork.CreateReadWriteRepository<Post>();

            var post = await postRepo
                .QueryById(request.PostId)
                .Include(p => p.Likes)
                .ThenInclude(p => p.UserProfile)
                .FirstAsync(cancellationToken);


            if (post.UserProfileId == request.CurrentUser)
            {
                foreach (var like in post.Likes)
                    like.SetLikeAsSeen();
            }

            result.Data = _mapper.Map<IReadOnlyList<GetLikesForAPostResponse>>(post.Likes);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
