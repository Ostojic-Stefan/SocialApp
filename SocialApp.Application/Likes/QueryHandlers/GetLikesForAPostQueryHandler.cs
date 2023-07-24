using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var postLikeRepo = _unitOfWork.CreateReadOnlyRepository<PostLike>();
            var postLikes = await postLikeRepo
                .Query()
                .Where(pl => pl.PostId == request.PostId)
                .ProjectTo<GetLikesForAPostResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            result.Data = postLikes;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
