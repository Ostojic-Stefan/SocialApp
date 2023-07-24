using AutoMapper;
using AutoMapper.QueryableExtensions;
using EfCoreHelpers;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Likes.Queries;
using SocialApp.Application.Likes.Responses;
using SocialApp.Application.Models;
using SocialApp.Domain;

namespace SocialApp.Application.Likes.QueryHandlers;

internal class GetLikesByUserQueryHandler
    : DataContextRequestHandler<GetLikesByUserQuery, Result<IReadOnlyList<PostLikeResponse>>>
{
    private readonly IMapper _mapper;

    public GetLikesByUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) 
        : base(unitOfWork)
    {
        _mapper = mapper;
    }

    public override async Task<Result<IReadOnlyList<PostLikeResponse>>> Handle(
        GetLikesByUserQuery request, CancellationToken cancellationToken)
    {
        var result = new Result<IReadOnlyList<PostLikeResponse>>();
        try
        {
            var postLikeRepo = _unitOfWork.CreateReadOnlyRepository<PostLike>();
            var userLikes = await postLikeRepo
                .Query()
                .Include(pl => pl.UserProfile)
                .Where(pl => pl.UserProfile.Username == request.Username)
                .ProjectTo<PostLikeResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            result.Data = userLikes;
        }
        catch (Exception ex)
        {
            result.AddError(AppErrorCode.ServerError, ex.Message);
        }
        return result;
    }
}
